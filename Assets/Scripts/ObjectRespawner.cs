using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectRespawner : MonoBehaviour
{
    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Quaternion> initialRotations = new Dictionary<GameObject, Quaternion>();

    // Add a public Transform for the special spawn location
    public Transform specialSpawnPoint;

    void Start()
    {
        // Find all pickupable objects in the level
        GameObject[] p = GameObject.FindGameObjectsWithTag("Pickupable");
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");

        GameObject[] pickupables = p.Concat(interactables).ToArray();
        foreach (GameObject obj in pickupables)
        {
            // Record their initial positions and rotations
            initialPositions[obj] = obj.transform.position;
            initialRotations[obj] = obj.transform.rotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is tagged as "Pickupable"
        if (other.CompareTag("Pickupable"))
        {
            Vector3 respawnPosition;
            Quaternion respawnRotation = Quaternion.identity; // Default rotation

            // Check if it's the 'pinecone' and if specialSpawnPoint is assigned
            if (other.gameObject.name == "pinecone" && specialSpawnPoint != null)
            {
                // Set the respawn position to be 5 units above the special spawn point
                respawnPosition = new Vector3(specialSpawnPoint.position.x, 
                                              specialSpawnPoint.position.y + 2, 
                                              specialSpawnPoint.position.z);
            }
            else if (initialPositions.TryGetValue(other.gameObject, out Vector3 initialPosition) &&
                     initialRotations.TryGetValue(other.gameObject, out Quaternion initialRotation))
            {
                // Use the initial position and rotation for all other objects
                respawnPosition = initialPosition;
                respawnRotation = initialRotation;
            }
            else
            {
                // If the object is not in the dictionary, do nothing more
                return;
            }

            // Respawn the object at the position and reset its orientation
            other.gameObject.transform.position = respawnPosition;
            other.gameObject.transform.rotation = respawnRotation;
            
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
