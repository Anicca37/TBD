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
        flower.transform.position = transform.position; // Snap to stone position
        var flowerRb = flower.GetComponent<Rigidbody>();
        if (flowerRb != null)
        {
            flowerRb.isKinematic = false; // Keep it movable
        }
    }

    void TriggerWindEffect()
    {
        Instantiate(windEffectPrefab, transform.position, Quaternion.identity);
    }

    void CheckAllFlowersMatched()
    {
        if (matchedFlowersCount >= totalFlowers)
        {
            GameManager.Instance.CompletePuzzle("Floral");
        }
    }
}
