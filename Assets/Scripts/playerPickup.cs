using UnityEngine;

public class playerPickup : MonoBehaviour
{
    public string pickupableTag = "Pickupable";
    public Transform attachPoint;

    private GameObject currentPickup;
    private Rigidbody currentPickupRb;
    public float pickupRange = 10f;

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            // Check if the player is not already carrying an object
            if (currentPickup == null)
            {
                PickUpObject();
            }
            else
            {
                DropObject();
            }
        }
    }

    void PickUpObject()
    {
        // Raycast to detect pickupable objects
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag(pickupableTag))
            {
                // Set the current pickup to the hit object
                currentPickup = hit.collider.gameObject;

                //disable physics
                currentPickupRb = currentPickup.GetComponent<Rigidbody>();
                if (currentPickupRb != null)
                {
                    currentPickupRb.isKinematic = true;
                }

                // Attach the pickup to the attachPoint
                currentPickup.transform.parent = attachPoint;
                // currentPickup.transform.localPosition = Vector3.zero;
                // currentPickup.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void DropObject()
    {
        // Detach the current pickup from the attachPoint
        currentPickup.transform.parent = null;

        // Re-enable physics for the dropped object
        if (currentPickupRb != null)
        {
            currentPickupRb.isKinematic = false;
        }

        // Reset the current pickup variable
        currentPickup = null;
    }
}
