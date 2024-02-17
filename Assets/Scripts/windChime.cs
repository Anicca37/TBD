using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;

public class windChime : MonoBehaviour
{
    public float swingStrength = 1000f; // adjust??
    public Vector3 directionOnClicked;
    private WindManager windManager;

    void Start()
    {
        windManager = FindObjectOfType<WindManager>();
    }

    void Update()
    {
        // right click on mouse
        if (Input.GetMouseButtonDown(1))
        {
            // cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit something: " + hit.collider.gameObject.name);

                // check if chime was clicked
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Chime was clicked, applying force."
                    + hit.collider.gameObject.name);
                    Rigidbody rb = gameObject.GetComponent<Rigidbody>();

                    // apply a force to the chime to swing it
                    if (rb != null)
                    {
                        rb.AddForce(Vector3.up * swingStrength, ForceMode.Impulse); // upward direction
                    }

                    // change wind direction w/ WindManager
                    if (windManager != null)
                    {
                        windManager.ChangeWindDirection(directionOnClicked);
                    }
                }
            }
        }
    }
}