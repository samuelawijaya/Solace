using UnityEngine;

public class StairsMovement : MonoBehaviour
{
    
    private const string LADDER = "Ladder";

    private Vector2 verticalVelocity;
    private bool isLadder;
    private bool isClimbing;

    [SerializeField] private float stairSpeed;
    [SerializeField] Player player;
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform groundCheck;

    private void Start()
    {
        gameInput.OnClimbStart += GameInput_OnClimbStart;
        gameInput.OnClimbEnd += GameInput_OnClimbEnd;
    }

    

    private void GameInput_OnClimbStart(object sender, System.EventArgs e)
    {
        verticalVelocity = gameInput.GetVerticalVectorNormalized();
    }

    private void GameInput_OnClimbEnd(object sender, System.EventArgs e)
    {
        verticalVelocity = new Vector2(rigidbody2D.linearVelocityX, 0.0f);
    }


    private void Update()
    {
        if(isLadder && Mathf.Abs(verticalVelocity.y) > 0f)
        {
            isClimbing = true;
        } else if (IsGrounded())
        {
            isClimbing = false;
        }

    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rigidbody2D.gravityScale = 0f;
            rigidbody2D.linearVelocity = new Vector2(0.0f, verticalVelocity.y * stairSpeed);
        }
        else
        {
            rigidbody2D.gravityScale = 5;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(LADDER))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(LADDER))
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);
    }

    public bool GetIsClimbing()
    {
        return isClimbing;
    }

    public bool GetIsClimbingAnimation()
    {
        return Mathf.Abs(verticalVelocity.y) > 0f;
    }


}
