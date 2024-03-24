using UnityEngine;

public class playerPickup : MonoBehaviour
{
    public string pickupableTag = "Pickupable";
    public Transform attachPoint;

    private GameObject currentPickup;
    private Rigidbody currentPickupRb;
    public GameObject defaultIcon;
    public GameObject grabIcon;
    public float pickupRange = 10f;

    void Update()
    {
        if (FPSInputManager.GetInteract())
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

    void SwitchIcon(bool highlight)
    {
        defaultIcon.SetActive(!highlight);
        grabIcon.SetActive(highlight);
    }

    void PickUpObject()
    {
        // Raycast to detect pickupable objects
        Camera playerCamera = Camera.main; // Get the main camera, assuming the player is using the main camera

        if(playerCamera == null)
        {
            return;
        }

        // Raycast from the camera, not from the player's position
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition); // Cast the ray from where the mouse pointer is on the screen

        if (Physics.Raycast(ray, out hit, pickupRange * 3))
        {
            if (hit.collider.CompareTag(pickupableTag))
            {
                // Calculate the horizontal distance to the hit object, ignoring the Y component
                Vector3 horizontalPlayerPosition = new Vector3(transform.position.x, 0f, transform.position.z);
                Vector3 hitPointHorizontal = new Vector3(hit.point.x, 0f, hit.point.z);
                float horizontalDistance = Vector3.Distance(horizontalPlayerPosition, hitPointHorizontal);

                // Check if the hit object is within the allowable Y distance
                float yDistance = Mathf.Abs(hit.point.y - transform.position.y);
                float playerHeight = 2f; // Arbitrary player height, adjust as necessary

                // Check if the object is within pickup range horizontally and vertically
                if (horizontalDistance <= pickupRange && yDistance <= playerHeight)
                {
                    // Set the current pickup to the hit object
                    currentPickup = hit.collider.gameObject;

                    //disable physics
                    currentPickupRb = currentPickup.GetComponent<Rigidbody>();
                    if (currentPickupRb != null)
                    {
                        SwitchIcon(true);
                        currentPickupRb.isKinematic = true;
                    }

                    // Attach the pickup to the attachPoint
                    currentPickup.transform.parent = attachPoint;
                    // currentPickup.transform.localPosition = Vector3.zero;
                    // currentPickup.transform.localRotation = Quaternion.identity;

                    if (currentPickup.name.Contains("flower"))
                    {
                        AkSoundEngine.PostEvent("Play_FlowerPickUp", this.gameObject);
                    }
                    else if (currentPickup.name.Contains("Chair"))
                    {
                        AkSoundEngine.PostEvent("Play_TablePickUp", this.gameObject);
                    }
                    else if (currentPickup.name.Contains("pinecone"))
                    {
                        AkSoundEngine.PostEvent("Play_PineconePickup", this.gameObject);
                    }
                    else if (currentPickup.name.Contains("block"))
                    {
                        AkSoundEngine.PostEvent("Play_BlockPickUp", this.gameObject);
                    }
                }
            }
        }
    }

    public void DropObject()
    {
        if (currentPickup == null)
        {
            return;
        }
        // Detach the current pickup from the attachPoint
        currentPickup.transform.parent = null;

        // Re-enable physics for the dropped object
        if (currentPickupRb != null)
        {
            SwitchIcon(false);
            currentPickupRb.isKinematic = false;
        }

        if (currentPickup.name.Contains("Chair"))
        {
            AkSoundEngine.PostEvent("Play_TableDrop", this.gameObject);
        }
        else if (currentPickup.name.Contains("block"))
        {
            AkSoundEngine.PostEvent("Play_BlockDrop", this.gameObject);
        }
        // Reset the current pickup variable
        currentPickup = null;
        
    }
}
