using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockMatch : MonoBehaviour
{
    public string colorName; 
    public static int matchedBlocks = 0;
    private const int totalBlocks = 5;
    public GameObject Confetti;

    public static void ResetmatchedBlocks()
    {
        matchedBlocks = 0;
    }

    void Start()
    {
        ResetmatchedBlocks();
    }

    private void OnTriggerEnter(Collider other)
    {
        FlowerScript flowerScript = other.GetComponent<FlowerScript>();
        if (flowerScript != null && flowerScript.colorName == this.colorName && !flowerScript.isMatched)
        {
            // Correct match
            flowerScript.isMatched = true; // Mark as matched
            SnapBlockToBucket(other.gameObject);
            
            // play sound
            AkSoundEngine.PostEvent("Play_Confetti", other.gameObject);

            TriggerConfetti();
            matchedBlocks++;
            AutomaticallyDropblock(other.gameObject);
            CheckAllBlocksMatched();
        }
    }


    void SnapBlockToBucket(GameObject block)
    {
        float yOffset = 1.5f; 

        var blockRb = block.GetComponent<Rigidbody>();
        if (blockRb != null)
        {
            blockRb.detectCollisions = false; 
            Destroy(blockRb); 
        }

        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z + 0.4f);
        block.transform.position = newPosition;
    }

    void TriggerConfetti()
    {
        Instantiate(Confetti, transform.position, Quaternion.identity);
    }


    void CheckAllBlocksMatched()
    {
        if (matchedBlocks >= totalBlocks)
        {
            PlayPlaceManager.Instance.CompletePuzzle("BlockSorting");
        }
    }

    void AutomaticallyDropblock(GameObject block)
    {
        playerPickup playerPickupScript = block.GetComponentInParent<playerPickup>();
        playerPickupScript.DropObject();
        block.tag = "attached";   
    }
}
