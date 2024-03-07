using UnityEngine;

public class PlayerLookDetection : MonoBehaviour
{
    public Transform cameraTransform; // Assign the player's camera transform in the inspector
    public float detectionRange = 10f; // Maximum range to detect objects
    private InteractableOutline lastInteractable; // Keep track of the last interactable object

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, detectionRange))
        {
            // Check if the hit object has the correct tag
            if (hit.collider.CompareTag("Pickupable") || hit.collider.CompareTag("Interactable"))
            {
                var interactable = hit.collider.GetComponent<InteractableOutline>();
                if (interactable != null)
                {
                    if (lastInteractable != interactable)
                    {
                        if (lastInteractable != null)
                        {
                            // Reset the last interactable object's outline
                            lastInteractable.SetIsLookingAt(false);
                        }
                        lastInteractable = interactable;
                        interactable.SetIsLookingAt(true);
                    }
                }
            }
            else
            {
                ResetLastInteractable();
            }
        }
        else
        {
            ResetLastInteractable();
        }
    }

    void ResetLastInteractable()
    {
        if (lastInteractable != null)
        {
            lastInteractable.SetIsLookingAt(false);
            lastInteractable = null;
        }
    }
}
