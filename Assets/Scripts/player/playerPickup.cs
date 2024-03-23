using UnityEngine;
using UnityEngine.InputSystem;

public class playerPickup : MonoBehaviour
{
    public string pickupableTag = "Pickupable";
    public Transform attachPoint; // The placeholder where picked objects should go
    public GameObject defaultIcon;
    public GameObject grabIcon;
    public float pickupRange = 10f;

    private GameObject currentPickup;
    private Rigidbody currentPickupRb;
    private PlayerInput playerInput;
    private InputAction interactAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); // The PlayerInput should be on the player, not the object being picked up
        interactAction = playerInput.actions["Interact"];
    }
    void Update()
    {
        // Check for the interact action instead of the old input
        if (interactAction.triggered)
        {
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
        // Cast a ray from the camera forward to check for pickups
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.collider.CompareTag(pickupableTag))
            {
                currentPickup = hit.collider.gameObject;
                currentPickupRb = currentPickup.GetComponent<Rigidbody>();
                if (currentPickupRb != null)
                {
                    currentPickupRb.isKinematic = true;
                    SwitchIcon(true);
                }

                // Store the original scale
                Vector3 originalScale = currentPickup.transform.localScale;

                // Set the parent
                currentPickup.transform.SetParent(attachPoint);

                // Reset local position and rotation
                currentPickup.transform.localPosition = Vector3.zero;
                currentPickup.transform.localRotation = Quaternion.identity;

                // Reapply the original scale
                currentPickup.transform.localScale = originalScale;

                PlaySoundBasedOnObjectName(currentPickup.name, true);
            }
        }
    }



    public void DropObject()
    {
        if (currentPickup == null) return;

        // Before detaching, reset to default parent's scale if needed, but usually, it's unnecessary for dropping
        currentPickup.transform.SetParent(null);

        if (currentPickupRb != null)
        {
            currentPickupRb.isKinematic = false;
            SwitchIcon(false);
        }

        PlaySoundBasedOnObjectName(currentPickup.name, false);

        currentPickup = null;
    }


    private void PlaySoundBasedOnObjectName(string objectName, bool isPickingUp)
    {
        string soundEventName = "";
        if (objectName.Contains("flower"))
        {
            soundEventName = isPickingUp ? "Play_FlowerPickUp" : "";
        }
        else if (objectName.Contains("Chair"))
        {
            soundEventName = isPickingUp ? "Play_TablePickUp" : "Play_TableDrop";
        }
        else if (objectName.Contains("pinecone"))
        {
            soundEventName = isPickingUp ? "Play_PineconePickup" : "";
        }
        else if (objectName.Contains("block"))
        {
            soundEventName = isPickingUp ? "Play_BlockPickUp" : "Play_BlockDrop";
        }

        if (!string.IsNullOrEmpty(soundEventName))
        {
            AkSoundEngine.PostEvent(soundEventName, gameObject);
        }
    }
}
