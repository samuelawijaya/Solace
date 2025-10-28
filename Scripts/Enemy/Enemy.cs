using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Enemy : Unit
{
    [SerializeField] private EnemySO enemySO;

    [SerializeField] private Player player;
    [SerializeField] private bool idle;
    [SerializeField] private Animator animator;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private int patrolDestination;
    [SerializeField] private float targetDistance;
    [SerializeField] private Transform[] patrolPoints;

    // NEW: how close in Y counts as "same floor"
    [SerializeField] private float floorTolerance = 1.0f;

    private bool isIdling;
    private float idleTimer;
    private float idleMax = 2f;

    private bool isAttacking;
    private bool isPlayerInRange;
    private float attackTimer;
    private Vector2 oldVector;


    private State state;

    public enum State
    {
        Patrol,
        Chase,
        Idle,
        Attack,
        Dead,
    }

    private void Start()
    {
        isPlayerInRange = false;

        if (idle == false)
        {
            state = State.Patrol;
            animator.SetBool("IsWalking", true);
        }
        else
        {
            state = State.Idle;
            animator.SetBool("IsWalking", false);
        }

        attackTimer = 0.0f;

        SetMoveDir(Vector2.zero);

        SetHealth(enemySO.health);

        idleTimer = 0.0f;
    }

    private void Update()
    {
        if (state == State.Dead) return; // Stop all movement logic when dead
        if (player.playerisDead)
        {
            state = State.Idle;
            isPlayerInRange = false;
            return;
        }
            


        SlopeCheck();
        //Debug.Log(IsOnSlope());

        switch (state)
        {
            case State.Idle:
                if (Vector2.Distance(transform.position, playerTransform.position) < enemySO.chaseDistance
                    && IsOnSameFloor())
                {
                    state = State.Chase;
                }
                break;

            case State.Patrol:
                if (Vector2.Distance(transform.position, playerTransform.position) < enemySO.chaseDistance
                    && IsOnSameFloor())
                {
                    state = State.Chase;
                }

                if (isIdling)
                {
                    idleTimer += Time.deltaTime;
                    if (idleTimer >= idleMax)
                    {
                        isIdling = false;
                        animator.SetBool("IsWalking", true);
                        idleTimer = 0f;
                    }
                    break;
                }

                if (patrolDestination == 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, enemySO.moveSpeed * Time.deltaTime);
                    if (Vector2.Distance(transform.position, patrolPoints[0].position) < targetDistance)
                    {
                        patrolDestination = 1;
                        StartIdle();
                    }
                }
                else if (patrolDestination == 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, enemySO.moveSpeed * Time.deltaTime);
                    if (Vector2.Distance(transform.position, patrolPoints[1].position) < targetDistance)
                    {
                        patrolDestination = 0;
                        StartIdle();
                    }
                }
                break;

            case State.Chase:
                // NEW: if player is on a different floor, immediately stop and return to patrol
                if (!IsOnSameFloor())
                {
                    SetupReturnToPatrol();
                    state = State.Patrol;
                    break;
                }

                if (player.playerisDead)
                {
                    state = State.Idle;
                    isPlayerInRange = false;
                    break;
                }

                animator.SetBool("IsWalking", true);

                if (transform.position.x > playerTransform.position.x)
                {
                    SetMoveDir(Vector2.left);

                    if (IsOnSlope() && GetCanWalkOnSlope())
                    {
                        Vector2 newVelocity = new Vector2(
                            enemySO.moveSpeed * GetSlopeNormalPerpendicular().x * (-GetMoveDir().x),
                            enemySO.moveSpeed * GetSlopeNormalPerpendicular().y * (-GetMoveDir().x));
                        GetRigidBody().linearVelocity = newVelocity;
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        GetRigidBody().linearVelocity = Vector3.left * enemySO.chaseSpeed;
                    }
                }
                else if (transform.position.x < playerTransform.position.x)
                {
                    SetMoveDir(Vector2.right);

                    if (IsOnSlope() && GetCanWalkOnSlope())
                    {
                        Vector2 newVelocity = new Vector2(
                            enemySO.moveSpeed * GetSlopeNormalPerpendicular().x * (-GetMoveDir().x),
                            enemySO.moveSpeed * GetSlopeNormalPerpendicular().y * (-GetMoveDir().x));
                        GetRigidBody().linearVelocity = newVelocity;
                    }
                    else
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        GetRigidBody().linearVelocity = Vector3.right * enemySO.chaseSpeed;
                    }
                }
                break;

            case State.Attack:
                if (!isAttacking)
                    StartCoroutine(PerformAttack());
                break;

            case State.Dead:
                
                break;
        }
    }

    public override void TakeDamage(int damage)
    {
        SetHealth(GetHealth() - damage);

        Debug.Log("Enemy Health health = " + GetHealth());

        animator.Play("Hit");

        if (GetHealth() <= 0)
        {
            state = State.Dead;
            animator.Play("Death");
            animator.SetBool("IsDead", true);

            // Stop all movement & physics
            GetRigidBody().linearVelocity = Vector2.zero;
            GetRigidBody().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            enabled = false; // completely stop AI logic
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        SetMoveDir(Vector2.zero);
        animator.Play("Attack");

        yield return new WaitForSeconds(enemySO.attackWindup);

        if (isPlayerInRange)
        {
            player.knockbackCounter = player.knockbackTotalTime;
            player.SetIsKnockedBack(true);
            player.knockFromRight = (transform.position.x >= player.transform.position.x);
            player.TakeDamage(enemySO.damage);
        }

        yield return new WaitForSeconds(enemySO.attackCooldown);
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("IsWalking", false);
            isPlayerInRange = true;
            state = State.Attack;
            //Debug.Log("Enemy Trigger Enter");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        animator.SetBool("IsWalking", true);

        attackTimer = 0.0f;
        isPlayerInRange = false;

        
        if (IsOnSameFloor())
        {
            state = State.Chase;
        }
        else
        {
            SetupReturnToPatrol();
            state = State.Patrol;
        }

        //Debug.Log("Enemy Trigger Exit");
    }

    private void StartIdle()
    {
        isIdling = true;
        animator.SetBool("IsWalking", false);
    }


    private bool IsOnSameFloor()
    {
        // Check slope condition first
        if (IsOnSlope() && GetCanWalkOnSlope() && player.IsOnSlope() && player.GetCanWalkOnSlope())
        {
            return true;
        }

        // Fallback: normal vertical tolerance check
        return Mathf.Abs(transform.position.y - playerTransform.position.y) <= floorTolerance;
    }

    private void SetupReturnToPatrol()
    {
        // stop any leftover chase motion this frame
        GetRigidBody().linearVelocity = Vector2.zero;
        SetMoveDir(Vector2.zero);

        // choose the closest patrol point so we don't U-turn needlessly
        if (patrolPoints != null && patrolPoints.Length >= 2)
        {
            float d0 = Vector2.Distance(transform.position, patrolPoints[0].position);
            float d1 = Vector2.Distance(transform.position, patrolPoints[1].position);
            patrolDestination = d0 <= d1 ? 0 : 1;

            // face it immediately to avoid a pop
            float dir = Mathf.Sign(patrolPoints[patrolDestination].position.x - transform.position.x);
            transform.localScale = new Vector3(dir >= 0 ? 1 : -1, 1, 1);
            //Debug.Log(dir);
        }

        animator.SetBool("IsWalking", true);

        
    }
}
