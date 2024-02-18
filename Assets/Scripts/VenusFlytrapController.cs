using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenusFlytrapController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the Venus Flytrap");
            GardenManager.Instance.CompletePuzzle("Escape");
        }
    }
}
