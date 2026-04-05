using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private InputSystem_Actions _inputActions;

    // Events
    #region

    public event Action OnCameraForward;
    public event Action OnCameraBack;

    public event Action OnCycleLeft;
    public event Action OnCycleRight;
    public event Action OnConfirmed;
    public event Action OnGoBack;

    #endregion

    public override void Awake()
    {
        base.Awake();
        _inputActions = new InputSystem_Actions();
        _inputActions.Enable();
    }

    private void OnEnable()
    {
        var p = _inputActions.Player;

        p.CameraForward.performed += CameraForwardPerformed;
        p.CameraBackward.performed += CameraBackPerformed;
        p.CycleLeft.performed += CycleLeftPerformed;
        p.CycleRight.performed += CycleRightPerformed;
        p.Confirmed.performed += ConfirmedPerformed;
        p.GoBack.performed += GoBackPerformed;

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        var p = _inputActions.Player;

        p.CameraForward.performed -= CameraForwardPerformed;
        p.CameraBackward.performed -= CameraBackPerformed;
        p.CycleLeft.performed -= CycleLeftPerformed;
        p.CycleRight.performed -= CycleRightPerformed;
        p.Confirmed.performed -= ConfirmedPerformed;
        p.GoBack.performed -= GoBackPerformed;

        _inputActions.Disable();
    }
    
    private void CameraForwardPerformed(InputAction.CallbackContext ctx)
        => OnCameraForward?.Invoke();

    private void CameraBackPerformed(InputAction.CallbackContext ctx)
        => OnCameraBack?.Invoke();

    private void CycleLeftPerformed(InputAction.CallbackContext ctx)
        => OnCycleLeft?.Invoke();

    private void CycleRightPerformed(InputAction.CallbackContext ctx)
        => OnCycleRight?.Invoke();

    private void ConfirmedPerformed(InputAction.CallbackContext ctx)
        => OnConfirmed?.Invoke();

    private void GoBackPerformed(InputAction.CallbackContext ctx)
        => OnGoBack?.Invoke();
}
