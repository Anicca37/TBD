using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSwap : MonoBehaviour
{
    public GameObject alternativeFlower;

    public void SwapFlower()
    {
        if (alternativeFlower != null)
        {
            // Activate the alternative flower
            alternativeFlower.SetActive(true);
            alternativeFlower.tag = "attached";

            // Deactivate the current flower
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Alternative flower is not assigned for " + gameObject.name);
        }
    }
}

