using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameInput;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Transform aimTransform;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private PlayerInventoryManager playerInventoryManager;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform playerModel;

    [SerializeField] private Transform gunSprite;
    [SerializeField] private Animator gunAnimator;


    [SerializeField] private AudioClip shootSoundClip;

    private Vector3 mouseWorldPos;
    private Vector3 aimDir;
    private float angle;

    private Camera mainCamera;

    private Vector2 mouseScreenPos;



    private void Awake()
    {
        mainCamera = Camera.main;
    }


    private void Start()
    {
        gameInput.OnShoot += GameInput_OnShoot;

    }

    private void GameInput_OnShoot(object sender, System.EventArgs e)
    {
        if (Time.timeScale == 0f) return;

        if (playerInventoryManager.GetLoadedAmmo() <= 0)
        {
            return; //Out of ammo
        }

        SoundManager.instance.PlaySoundClip(shootSoundClip, transform, 1f);


        RaycastHit2D hit = Physics2D.Raycast(gunBarrel.position, aimDir.normalized, 100f, targetLayer);

        playerInventoryManager.shootAmmo();
        gunAnimator.Play("Shoot", -1, 0f);

        if (hit.collider != null )
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            Window window = hit.collider.GetComponent<Window>();

            //GameObject hitEnemy = hit.collider.gameObject;

            Debug.DrawLine(gunBarrel.position, hit.point, Color.red, 1f);

            

            if (enemy != null) 
            {
                enemy.TakeDamage(4);
                Debug.Log("Hit enemy");
            }
            else if(window != null) 
            {
                window.BreakWindow();
                Debug.Log("Hit window");
            }
            else
            {
                Debug.Log("Hit Other");
            }


        } 
    }

    private void Update()
    {
        HandleAiming();
    }


    private void HandleAiming()
    {
        if (Time.timeScale == 0f) return;
        mouseScreenPos = gameInput.GetMouseLocation();

        mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        aimDir = mouseWorldPos - aimTransform.position;
        angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        // Rotate the gun
        aimTransform.rotation = Quaternion.Euler(0, 0, angle);

        if (aimDir.x >= 0f)
        {
            playerModel.localScale = new Vector3(1, 1, 1);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            playerModel.localScale = new Vector3(-1, 1, 1);
            transform.localScale = new Vector3(-1, -1, 1);

        }
    }

}
