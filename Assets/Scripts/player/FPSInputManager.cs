using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInputManager : MonoBehaviour
{
    private static FPSControl inputActions;

    private void Awake()
    {
        inputActions = new FPSControl();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public static Vector2 GetPlayerMovement()
    {
        return inputActions.FPSController.Move.ReadValue<Vector2>();
    }

    public static Vector2 GetPlayerLook()
    {
        return inputActions.FPSController.Look.ReadValue<Vector2>();
    }

    public static bool GetJump()
    {
        return inputActions.FPSController.Jump.triggered;
    }

    public static bool GetInteract()
    {
        return inputActions.FPSController.Interact.triggered;
    }

    public static bool GetCancel()
    {
        return inputActions.FPSController.Cancel.triggered;
    }

    public static bool GetMouseDownHelper()
    {
        return inputActions.FPSController.MouseDownHelper.triggered;
    }
}
