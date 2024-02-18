using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMatch : MonoBehaviour
{
    public string colorName; // Assign in the Inspector
    public GameObject windEffectPrefab; // Assign your wind effect prefab here
    public static int matchedFlowersCount = 0;
    private const int totalFlowers = 5; // Total number of flowers to match

    private void OnTriggerEnter(Collider other)
    {
        FlowerScript flowerScript = other.GetComponent<FlowerScript>();
        if (flowerScript != null && flowerScript.colorName == this.colorName && !flowerScript.isMatched)
        {
            // Correct match
            flowerScript.isMatched = true; // Mark as matched
            SnapFlowerToStone(other.gameObject); // Snap flower to stone
            TriggerWindEffect();
            matchedFlowersCount++;
            CheckAllFlowersMatched();
        }
    }

    void SnapFlowerToStone(GameObject flower)
    {
        float yOffset = 1f; 

        var flowerRb = flower.GetComponent<Rigidbody>();
        if (flowerRb != null)
        {
            flowerRb.detectCollisions = false; 
            Destroy(flowerRb); 
        }

        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        flower.transform.position = newPosition;
    }

    void TriggerWindEffect()
    {
        Instantiate(windEffectPrefab, transform.position, Quaternion.identity);
    }

    void CheckAllFlowersMatched()
    {
        if (matchedFlowersCount >= totalFlowers)
        {
            GardenManager.Instance.CompletePuzzle("Floral");
        }
    }
}
