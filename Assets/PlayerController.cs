using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    public float interactionDistance = 100f;
    public string interactableTag = "Interactable"; // The tag you're using for interactable objects
    private PlayerInput playerInput;
    private InputAction interactAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); // Ensure this script is on the same GameObject as the PlayerInput component
        interactAction = playerInput.actions["Interact"]; // Ensure you have an "Interact" action in your input asset
    }

    private void Update()
    {
        if (interactAction.triggered)
        {
            AttemptInteraction();
        }
    }

    private void AttemptInteraction()
    {
        RaycastHit hit;
        // Cast a ray from the camera in the direction it's facing
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Check if the raycast hit an object with the specified tag
            if (hit.collider.CompareTag(interactableTag))
            {
                // Call the Interact method on the hit object if it has the buttonScript component
                buttonScript button = hit.collider.GetComponent<buttonScript>();
                if (button != null)
                {
                    button.Interact();
                }
            }
        }
    }
}
