using UnityEngine;

public class ObjectLookChecker : MonoBehaviour
{
    public float raycastDistance = 10f;

    private Outline testbugScript; // Reference to the Testbug script on the currently looked-at object

    void Update()
    {
        // Get the player's camera
        Camera playerCamera = Camera.main;

        if (playerCamera != null)
        {
            // Create a ray from the camera's position and forward direction
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            // Declare a RaycastHit variable to store information about the hit
            RaycastHit hit;

            // Check if the ray hits an object within the specified distance
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Check if the hit object has a specific tag (you can customize this condition)
                if (hit.collider.CompareTag("Pickupable") || hit.collider.CompareTag("Interactable"))
                {
                    // Player is looking at the object
                    // Debug.Log("Player is looking at the object!");

                    // Try to get the "Testbug" script on the hit object
                    testbugScript = hit.collider.GetComponent<Outline>();

                    // Check if the script is found
                    if (testbugScript != null)
                    {
                        // Enable the "Testbug" script on the object
                        testbugScript.enabled = true;
                    }
                }
            }
            else
            {
                // Player is not looking at any object, so disable the script if it was previously enabled
                if (testbugScript != null)
                {
                    testbugScript.enabled = false;
                    testbugScript = null; // Reset the reference
                }
            }
        }
    }
}
