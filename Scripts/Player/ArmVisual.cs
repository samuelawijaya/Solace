using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameInput;

public class ArmVisual : MonoBehaviour
{
    [SerializeField] private Transform armTransform;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform gunBarrel;

    private Vector3 mouseWorldPos;
    private Vector3 armDirection;
    private float angle;

    private Camera mainCamera;
    private Vector2 mouseScreenPos;

    private Vector3 gunPosition;


    private void Awake()
    {
        mainCamera = Camera.main;
    }


    private void Start()
    {

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


        gunPosition = gunBarrel.position;

        //mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        gunPosition.z = 0f;

        armDirection = gunPosition - armTransform.position;
        angle = Mathf.Atan2(armDirection.y, armDirection.x) * Mathf.Rad2Deg;

        // Rotate the gun
        armTransform.rotation = Quaternion.Euler(0, 0, angle);

        if (mouseWorldPos.x - playerModel.position.x >= 0f)
        {
            //playerModel.localScale = new Vector3(1, 1, 1);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            //playerModel.localScale = new Vector3(-1, 1, 1);
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

}
