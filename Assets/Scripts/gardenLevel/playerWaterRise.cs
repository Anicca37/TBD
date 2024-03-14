using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWaterRise : MonoBehaviour
{
    public float riseSpeed = 2f;
    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // This moves the player up at the same speed as the water rises.
            // Adjust this to match your water's rising speed or desired effect.
            other.transform.position += Vector3.up * Time.deltaTime * riseSpeed; 
        }
    }
}
