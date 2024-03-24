using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 18f;
    public float movementSpeed; // Current movement speed
    public float jumpHeight = 2f;
    public float gravity = -60f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 movement = FPSInputManager.GetPlayerMovement();

        float x = movement.x;
        float z = movement.y;

        Vector3 move = transform.right * x + transform.forward * z;

        movementSpeed =  walkSpeed;

        controller.Move(move * movementSpeed * Time.deltaTime);

        if (FPSInputManager.GetJump() && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
