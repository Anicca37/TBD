using UnityEngine;

public class playerLookAtHighlighter : MonoBehaviour
{
    public float raycastDistance = 10f;

    private Outline outlineScript; // Reference to the Testbug script on the currently looked-at object

    void Update()
    {
        // Get the player's camera
        Camera playerCamera = Camera.main;

        if (playerCamera != null)
        {
            // Create a ray from the camera's position and forward direction
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            RaycastHit hit;

            // Check if the ray hits an object within the specified distance
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Check if the hit object has a specific tag
                if (hit.collider.CompareTag("Pickupable") || hit.collider.CompareTag("Interactable"))
                {

                    // Try to get the "Outline" script on the hit object
                    outlineScript = hit.collider.GetComponent<Outline>();

                    // Check if the script is found
                    if (outlineScript != null)
                    {
                        // Enable the "Outline" script on the object
                        outlineScript.enabled = true;
                    }
                }
            }
            else
            {
                // Player is not looking at any object, so disable the script if it was previously enabled
                if (outlineScript != null)
                {
                    outlineScript.enabled = false;
                    outlineScript = null; // Reset the reference
                }
            }
        }
    }
}
