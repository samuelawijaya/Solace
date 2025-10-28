using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GameInput : MonoBehaviour
{

    public event EventHandler OnClimbStart;
    public event EventHandler OnClimbEnd;
    public event EventHandler OnSprintStart;
    public event EventHandler OnSprintCancel;
    public event EventHandler OnDropStart;
    public event EventHandler OnDropEnd;

    public event EventHandler OnShoot;

    public event EventHandler OnInteract;

    public event EventHandler OnReload;

    public event EventHandler OnHeal;

    public event EventHandler OnPause;

    private PlayerInputActions playerInputActions;

    

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Climb.started += Climb_started;
        playerInputActions.Player.Climb.canceled += Climb_canceled;

        playerInputActions.Player.Sprint.started += Sprint_started;
        playerInputActions.Player.Sprint.canceled += Sprint_canceled;

        playerInputActions.Player.Drop.started += Drop_started;
        playerInputActions.Player.Drop.canceled += Drop_canceled;


        playerInputActions.Player.Shoot.performed += Shoot_performed;

        playerInputActions.Player.Interact.performed += Interact_performed;

        playerInputActions.Player.Reload.performed += Reload_performed;

        playerInputActions.Player.Heal.performed += Heal_performed;

        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        if (OnPause != null)
        {
            OnPause?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Heal_performed(InputAction.CallbackContext obj)
    {
        if (OnHeal != null)
        {
            OnHeal?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Reload_performed(InputAction.CallbackContext obj)
    {
        if (OnReload != null)
        {
            OnReload?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        if (OnInteract != null)
        {
            OnInteract?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Shoot_performed(InputAction.CallbackContext obj)
    {
        if (OnShoot != null)
        {
            OnShoot?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Drop_started(InputAction.CallbackContext obj)
    {
        if (OnDropStart != null)
        {
            OnDropStart?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Drop_canceled(InputAction.CallbackContext obj)
    {
        if (OnDropEnd != null)
        {
            OnDropEnd?.Invoke(this, EventArgs.Empty);
        }
    }

    
    private void Climb_started(InputAction.CallbackContext obj)
        {
            if (OnClimbStart != null)
            {
                OnClimbStart?.Invoke(this, EventArgs.Empty);
            }
        }


    private void Climb_canceled(InputAction.CallbackContext obj)
        {
             if (OnClimbEnd != null)
             {
                    OnClimbEnd?.Invoke(this, EventArgs.Empty);
             }
        }

    

    private void Sprint_started(InputAction.CallbackContext obj)
        {
            if (OnSprintStart != null)
            {
                OnSprintStart?.Invoke(this, EventArgs.Empty);
            }
        }

    private void Sprint_canceled(InputAction.CallbackContext obj)
        {
            if (OnSprintCancel != null)
            {
                OnSprintCancel?.Invoke(this, EventArgs.Empty);
            }
        }

    

    

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.LeftRight.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public Vector2 GetVerticalVectorNormalized()
    {
        Vector2 climbVector = playerInputActions.Player.Climb.ReadValue<Vector2>();

        climbVector = climbVector.normalized;

        return climbVector;
    }

    
    public Vector2 GetMouseLocation()
    {
        Vector2 mouseLocation = playerInputActions.Player.Aim.ReadValue<Vector2>();

        return mouseLocation;
    }

    public void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    
}
