using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : Unit
{
    [SerializeField] private PlayerInventoryManager playerInventoryManager;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator transition;
    private const string ISWALKING = "IsWalking";

    [SerializeField] private GameInput gameInput;
    [SerializeField] Transform lookPoint;
    [SerializeField] private StairsMovement stairMovement;

    

    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float sprintSpeed = 0.5f;
    //[SerializeField] private float jumpHeight = 1.0f;

    [SerializeField] private LayerMask doorLayer;

    private Vector3 mouseWorldPos;
    private Vector2 mouseScreenPos;
    private Camera mainCamera;


    private bool isSprinting;

    private bool isMoving;

    private Locker lockerInRange;
    private const string LOCKER = "Locker";

    

    private Vector2 inputVector;
    private Vector2 oldVector;
    

    [SerializeField] private int maxHealth = 10;

    [SerializeField] private float knockbackForce;

    //Public
    public float knockbackCounter;
    public bool knockFromRight;
    public float knockbackTotalTime;
    public bool playerisDead;


    //Sounds

    [SerializeField] AudioClip damagedSound;
    [SerializeField] AudioClip healSound;
    [SerializeField] AudioClip walkGrassSound;
    [SerializeField] AudioClip walkGroundSound;


    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        mainCamera = Camera.main;
        playerisDead = false;

    }

    private void Start()
    {
        //gameInput.OnJumpStart += GameInput_OnJumpStart;
        //gameInput.OnJumpEnd += GameInput_OnJumpEnd;


        oldVector = new Vector2(0f,0f);

        gameInput.OnSprintStart += GameInput_OnSprintStart;
        gameInput.OnSprintCancel += GameInput_OnSprintCancel;
        gameInput.OnInteract += GameInput_OnInteract_Door;
        gameInput.OnReload += GameInput_OnReload;
        gameInput.OnHeal += GameInput_OnHeal;

        SetHealth(maxHealth);
    }

    

    private void FixedUpdate()
    {
        SlopeCheck();

        mouseScreenPos = gameInput.GetMouseLocation();

        mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        float mouseAimDirection = mouseWorldPos.x - transform.position.x;


        float moveSpeed = isSprinting ? sprintSpeed : walkSpeed;

        inputVector = gameInput.GetMovementVectorNormalized();
        SetMoveDir(inputVector);

        if (stairMovement.GetIsClimbingAnimation())
        {
            animator.SetBool(ISWALKING, true);
            animator.SetBool("IsWalkingBackwards", false);
        }
        else if (inputVector.x > 0 && mouseAimDirection > 0)
        {
            animator.SetBool(ISWALKING, true);
            animator.SetBool("IsWalkingBackwards", false);
        }
        else if (inputVector.x > 0 && mouseAimDirection < 0)
        {
            animator.SetBool(ISWALKING, false);
            animator.SetBool("IsWalkingBackwards", true);
        }
        else if (inputVector.x < 0 && mouseAimDirection > 0)
        {
            animator.SetBool(ISWALKING, false);
            animator.SetBool("IsWalkingBackwards", true);
        }
        else if (inputVector.x < 0 && mouseAimDirection < 0)
        {
            animator.SetBool(ISWALKING, true);
            animator.SetBool("IsWalkingBackwards", false);
        }
        else
        {
            animator.SetBool(ISWALKING, false);
            animator.SetBool("IsWalkingBackwards", false);
        }

        //if (inputVector != Vector2.zero || stairMovement.GetIsClimbingAnimation())
        //{
        //    animator.SetBool(ISWALKING, true);
        //}
        //else
        //{
        //    animator.SetBool(ISWALKING, false);
        //}

        Vector2 moveVelocity = inputVector * moveSpeed;

        if(oldVector != inputVector && inputVector != Vector2.zero)
        {
            oldVector = inputVector;
        }


        //if(oldVector.x > 0f)
        //{
        //    transform.localScale = new Vector3(1f, 1f , 1f);
        //}
        //else if (oldVector.x < 0)
        //{
        //    transform.localScale = new Vector3(-1f, 1f , 1f);
        //}


        if (IsGrounded() && !IsOnSlope() && knockbackCounter <= 0)
        {
            GetRigidBody().linearVelocity = new Vector2(moveVelocity.x, GetRigidBody().linearVelocityY);

        } else if(IsGrounded() && IsOnSlope() && GetCanWalkOnSlope() && !GetIsKnockedBack())
        {
            Vector2 newVelocity = new Vector2(moveSpeed * GetSlopeNormalPerpendicular().x * (-inputVector.x), moveSpeed * GetSlopeNormalPerpendicular().y * (-inputVector.x));
            GetRigidBody().linearVelocity = newVelocity;
        }
        else if(knockFromRight && knockbackCounter > 0)
        {
            GetRigidBody().linearVelocity = new Vector2(-knockbackForce, knockbackForce);
        }
        else if (!knockFromRight && knockbackCounter > 0)
        {
            GetRigidBody().linearVelocity = new Vector2(knockbackForce, knockbackForce);
        }

        if (knockbackCounter > 0) 
        {
            knockbackCounter -= Time.deltaTime;
        }
        else
        {
            SetIsKnockedBack(false);
        }

        

    }

    private void Update()
    {
        
    }





    public override void TakeDamage(int damage)
    {
        SoundManager.instance.PlaySoundClip(damagedSound, transform, 1f);

        SetHealth(GetHealth() - damage);

        playerInventoryManager.SetHealthBar(GetHealth());

        Debug.Log("Player health = " + GetHealth());

        if (GetHealth() <= 0)
        {
            Debug.LogError("Player has died");
            Die();
        }
    }



    private void GameInput_OnInteract_Door(object sender, System.EventArgs e)
    {


        //if(lockerInRange != null && !lockerInRange.GetIsOpened())
        //{
        //    lockerInRange.Interact();
        //    return;
        //}


        Vector2 direction = oldVector;

        RaycastHit2D look = Physics2D.Raycast(lookPoint.position, direction, 2.0f, doorLayer);

        if(look.collider != null)
        {
            Door door = look.collider.GetComponent<Door>();
            if (door != null) 
            {
                door.ToggleDoor();
            }
        }
    }

    private void Die()
    {
        GameObject ArmLeft;
        GameObject ArmRight;


        ArmLeft = transform.Find("ArmLong").gameObject;
        ArmRight = transform.Find("ArmShort").gameObject;

        ArmLeft.SetActive(false);
        ArmRight.SetActive(false);
        Debug.LogError("Player has died");

        // Play death animation
        animator.Play("Death");
        transition.SetTrigger("Start");
        playerisDead = true;


        // Disable player controls (optional)
        gameInput.OnDisable();

        // Restart scene after delay (adjust time to your death animation length)
        StartCoroutine(RestartSceneAfterDelay(5f));
    }


    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void GameInput_OnSprintStart(object sender, System.EventArgs e)
        {
            isSprinting = true;
        }

    private void GameInput_OnSprintCancel(object sender, System.EventArgs e)
        {
            isSprinting = false;
        }

    private void GameInput_OnReload(object sender, System.EventArgs e)
    {
        if (Time.timeScale == 0f) return;
        playerInventoryManager.ReloadAmmo();
    }


    private void GameInput_OnHeal(object sender, System.EventArgs e)
    {
        if (Time.timeScale == 0f) return;
        if (playerInventoryManager.GetHealthPackAmount() > 0) 
        {
            SoundManager.instance.PlaySoundClip(healSound, transform, 1f);
            SetHealth(10);
            playerInventoryManager.SetHealthBar(GetHealth());
            playerInventoryManager.AddHealthPack(-1);
        }
        

    }

}

//[SerializeField] private Rigidbody2D rigidbody2D;

//[SerializeField] private Transform groundCheck;

//[SerializeField] private LayerMask whatIsGround;

//[SerializeField] private float slopeCheckDistance;
//[SerializeField] private PhysicsMaterial2D noFriction;
//[SerializeField] private PhysicsMaterial2D fullFriction;

//[SerializeField] private float maxSlopeAngle;

//private float slopeDownAngle;
//private float slopeDownAngleOld;
//private float slopeSideAngle;
//private Vector2 slopeNormalPerpendicular;

//private bool isOnSlope;

//private bool canWalkOnSlope;

/*
    private void SlopeCheck()
    {
        SlopeCheckHorizontal(groundCheck.position);
        SlopeCheckVertical(groundCheck.position);

        
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack) 
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else 
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPos) 
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {
            slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }

                slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.red);
                .DrawRay(hit.point, hit.normal, Color.green);
        }

        if(slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle) 
        { 
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && inputVector.x == 0.0f && canWalkOnSlope)
        {
            rigidbody2D.sharedMaterial = fullFriction;
        }
        else
        {
            rigidbody2D.sharedMaterial = noFriction;
        }
    }

    */
