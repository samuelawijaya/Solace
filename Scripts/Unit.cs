using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float groundRadius;

    [SerializeField] private bool isGrounded;

    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;

    [SerializeField] private float maxSlopeAngle;

    private int health;
    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float slopeSideAngle;
    private Vector2 slopeNormalPerpendicular;

    private bool isOnSlope;
    private bool canWalkOnSlope;
    private Vector2 moveDir;

    private bool isKnockedBack;


    public virtual void TakeDamage(int damage)
    {

    }
    public void SlopeCheck()
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

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && moveDir.x == 0.0f && canWalkOnSlope)
        {
            rigidbody2D.sharedMaterial = fullFriction;
        }
        else
        {
            rigidbody2D.sharedMaterial = noFriction;
        }
    }

    public void SetMoveDir(Vector2 inputVector)
    {
        moveDir = inputVector;
    }

    public Vector2 GetSlopeNormalPerpendicular()
    {
        return slopeNormalPerpendicular;
    }

    public bool GetCanWalkOnSlope()
    {
        return canWalkOnSlope;
    }

    public Rigidbody2D GetRigidBody()
    {
        return rigidbody2D;
    }

    public Transform GetGroundCheck()
    {
        return groundCheck;
    }

    public LayerMask GetWhatIsGround()
    {
        return whatIsGround;
    }

    public bool IsOnSlope()
    {
        return isOnSlope;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GetGroundCheck().position, groundRadius, GetWhatIsGround());
    }

    public float GetGroundRadius()
    {
        return groundRadius;
    }

    public Vector2 GetMoveDir()
    {
        return moveDir;
    }

    public void SetFrictionOff()
    {
        rigidbody2D.sharedMaterial = noFriction;
    }

    public void SetFrictionOn()
    {
        rigidbody2D.sharedMaterial = fullFriction;
    }

    public void SetIsKnockedBack(bool isAttacked)
    {
        isKnockedBack = isAttacked;
    }

    public bool GetIsKnockedBack()
    {
        return isKnockedBack;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int setHealth)
    {
        health = setHealth;
    }
}
