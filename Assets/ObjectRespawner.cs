using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawner : MonoBehaviour
{
    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        // Find all pickupable objects in the level
        GameObject[] pickupables = GameObject.FindGameObjectsWithTag("Pickupable");
        foreach (GameObject obj in pickupables)
        {
            // Record their initial positions
            initialPositions[obj] = obj.transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is tagged as "Pickupable"
        if (other.CompareTag("Pickupable"))
        {
            // If the object has fallen through the map and is in the initialPositions dictionary
            if (initialPositions.TryGetValue(other.gameObject, out Vector3 initialPosition))
            {
                // Respawn the object at its initial position
                other.gameObject.transform.position = initialPosition;
                
                
                // Optional: reset velocity if the object has a Rigidbody
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
    }
}
