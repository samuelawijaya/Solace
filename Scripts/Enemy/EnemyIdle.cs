using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EnemyIdle : Unit
{
    [SerializeField] private EnemySO enemySO;

    [SerializeField] private Player player;
    [SerializeField] private Animator animator;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private int patrolDestination;
    [SerializeField] private float targetDistance;
    [SerializeField] private Transform[] patrolPoints;

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
        state = State.Idle;
        attackTimer = 0.0f;

        SetMoveDir(Vector2.zero);

        SetHealth(enemySO.health);

        //idleTimer = 0.0f;
        animator.SetBool("IsWalking", false);
    }



    private void Update()
    {
        SlopeCheck();

        //Debug.Log(state);


            switch (state)
            {
                case State.Idle:

                    if (Vector2.Distance(transform.position, playerTransform.position) < enemySO.chaseDistance)
                    {
                        state = State.Chase;
                    }

                break;

                case State.Patrol:

                    break;

                case State.Chase:
                animator.SetBool("IsWalking", true);
                    if (transform.position.x > playerTransform.position.x)
                    {
                        SetMoveDir(Vector2.left);

                        if (IsOnSlope() && GetCanWalkOnSlope())
                        {
                            Vector2 newVelocity = new Vector2(enemySO.moveSpeed * GetSlopeNormalPerpendicular().x * (-GetMoveDir().x), enemySO.moveSpeed * GetSlopeNormalPerpendicular().y * (-GetMoveDir().x));
                            GetRigidBody().linearVelocity = newVelocity;
                        }
                        else
                        {
                            transform.localScale = new Vector3(-1, 1, 1);
                            //transform.position += Vector3.left * enemySO.chaseSpeed * Time.deltaTime;
                            GetRigidBody().linearVelocity = Vector3.left * enemySO.chaseSpeed;
                        }

                    }

                    if (transform.position.x < playerTransform.position.x)
                    {
                        SetMoveDir(Vector2.right);

                        if (IsOnSlope() && GetCanWalkOnSlope())
                        {
                            Vector2 newVelocity = new Vector2(enemySO.moveSpeed * GetSlopeNormalPerpendicular().x * (-GetMoveDir().x), enemySO.moveSpeed * GetSlopeNormalPerpendicular().y * (-GetMoveDir().x));
                            GetRigidBody().linearVelocity = newVelocity;
                        }
                        else
                        {
                            transform.localScale = new Vector3(1, 1, 1);
                            //transform.position += Vector3.right * enemySO.chaseSpeed * Time.deltaTime;
                            GetRigidBody().linearVelocity = Vector3.right * enemySO.chaseSpeed;
                        }

                    }

                    break;

                case State.Attack:

                if (!isAttacking)
                    StartCoroutine(PerformAttack());

                //attackTimer += Time.deltaTime;
                //SetMoveDir(Vector2.zero);

                //animator.Play("Attack");

                //if (attackTimer >= enemySO.attackSpeed && isPlayerInRange)
                //{
                //    player.knockbackCounter = player.knockbackTotalTime;
                //    player.SetIsKnockedBack(true);


                //    if (transform.position.x <= transform.position.x)
                //    {
                //        player.knockFromRight = true;
                //    }
                //    else if (transform.position.x > transform.position.x)
                //    {
                //        player.knockFromRight = false;
                //    }

                //    player.TakeDamage(enemySO.damage);
                //    //Debug.Log("Enemy Attack!, health = " + player.GetHealth());

                //    attackTimer = 0.0f;
                //}


                break;

                case State.Dead:
                GetComponent<CapsuleCollider2D>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Rigidbody2D>().simulated = false;

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
            player.knockFromRight = (transform.position.x <= player.transform.position.x);
            player.TakeDamage(enemySO.damage);
        }

        yield return new WaitForSeconds(enemySO.attackCooldown);
        isAttacking = false;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            animator.SetBool("IsWalking", false);

            isPlayerInRange = true;
            state = State.Attack;
            Debug.Log("Enemy Trigger Enter");

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("IsWalking", true);

        attackTimer = 0.0f;
        isPlayerInRange = false;
        state = State.Chase;

        Debug.Log("Enemy Trigger Exit");
    }

    private void StartIdle()
    {
        isIdling = true;
        animator.SetBool("IsWalking", false);
    }

}
