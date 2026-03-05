using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions _inputActions;

    // Events
    #region

    public event Action OnCameraForward;
    public event Action OnCameraBack;

    #endregion

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Enable();
    }

    private void OnEnable()
    {
        var p = _inputActions.Player;

        p.CameraForward.performed += CameraForwardPerformed;
        p.CameraBackward.performed += CameraBackPerformed;

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        var p = _inputActions.Player;

        p.CameraForward.performed -= CameraForwardPerformed;
        p.CameraBackward.performed -= CameraBackPerformed;

        _inputActions.Disable();
    }

    
    private void CameraForwardPerformed(InputAction.CallbackContext ctx)
    {
        OnCameraForward?.Invoke();
    }

    private void CameraBackPerformed(InputAction.CallbackContext ctx)
    {
        OnCameraBack?.Invoke();
    }
}
