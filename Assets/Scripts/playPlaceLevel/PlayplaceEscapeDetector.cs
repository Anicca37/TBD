using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayplaceEscapeDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the tunnel");
            PlayPlaceManager.Instance.CompletePuzzle("Escape");
        }
    }
}
