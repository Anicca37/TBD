using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    private PlayerInput _playerInput;

    // Several UI input actions
    private InputAction _pauseMenuOpenCloseAction;
    private InputAction _selectionUpAction;
    private InputAction _selectionDownAction;
    private InputAction _selectionLeftAction;
    private InputAction _selectionRightAction;
    private InputAction _confirmAction;


    // Several bools to check if the UI input is pressed
    public bool PauseMenuOpenCloseInput { get; private set; }
    public bool SelectionUpInput { get; private set; }
    public bool SelectionDownInput { get; private set; }
    public bool SelectionLeftInput { get; private set; }
    public bool SelectionRightInput { get; private set; }
    public bool ConfirmInput { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _playerInput = GetComponent<PlayerInput>();
        _pauseMenuOpenCloseAction = _playerInput.actions["PauseMenuOpenClose"];
        _selectionUpAction = _playerInput.actions["SelectionUp"];
        _selectionDownAction = _playerInput.actions["SelectionDown"];
        _selectionLeftAction = _playerInput.actions["SelectionLeft"];
        _selectionRightAction = _playerInput.actions["SelectionRight"];
        _confirmAction = _playerInput.actions["Confirm"];
    }

    private void Update()
    {
        PauseMenuOpenCloseInput = _pauseMenuOpenCloseAction.WasPressedThisFrame();
        SelectionUpInput = _selectionUpAction.WasPressedThisFrame();
        SelectionDownInput = _selectionDownAction.WasPressedThisFrame();
        SelectionLeftInput = _selectionLeftAction.WasPressedThisFrame();
        SelectionRightInput = _selectionRightAction.WasPressedThisFrame();
        ConfirmInput = _confirmAction.WasPressedThisFrame();
    }
}
