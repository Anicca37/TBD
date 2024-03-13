using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPineconeToScale : MonoBehaviour
{
    public bool isPineconeAttached = false;
    [SerializeField] private GameObject pinecone; // The pinecone object with the Rigidbody
    [SerializeField] private Transform pineconeMesh; // The child mesh of the pinecone
    [SerializeField] private Transform snapPoint; // The transform to which the pinecone mesh will snap

    public playerPickup playerPickupScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == pinecone && !isPineconeAttached)
        {
            AttachPineconeMesh();

        }
    }

    void AttachPineconeMesh()
    {
        isPineconeAttached = true;

        // Position the pinecone mesh at the snap point and parent it
        pineconeMesh.position = snapPoint.position;
        pineconeMesh.rotation = snapPoint.rotation;
        pineconeMesh.SetParent(snapPoint, true);

        // Optionally, here you can disable the pinecone's collider if you want to prevent it from interacting further
        // Collider pineconeCollider = pinecone.GetComponent<Collider>();
        // if (pineconeCollider != null)
        // {
        //     pineconeCollider.enabled = false;
        // }

        // If there's any logic in your player pickup script that needs to run when the pinecone is dropped, call it here
        if (playerPickupScript != null)
        {
            playerPickupScript.DropObject(); // Custom method to drop the object
        }
        Invoke("CompleteScalePuzzle", 0f);
    }

    void CompleteScalePuzzle()
    {
        GardenManager.Instance.CompletePuzzle("Scales");
    }
    
}
