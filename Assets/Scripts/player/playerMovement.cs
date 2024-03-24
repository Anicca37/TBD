using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 18f;
    public float jumpHeight = 2f;
    public float gravity = -60f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    public fpsCameraControl cameraControl;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Update()
    {
        CheckGrounded();
        ProcessMovement();
        ProcessJump();
        ApplyGravity();

        Vector2 lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        cameraControl.ProcessLook(lookInput);
    }

    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Make sure player sticks to the ground.
        }
    }

    void ProcessMovement()
    {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.y;
        controller.Move(move * walkSpeed * Time.deltaTime);
    }

    void ProcessJump()
    {
        if (jumpAction.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
