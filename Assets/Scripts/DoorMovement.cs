using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    public Transform[] clockControllers;
    public float minAngle = 90f;
    public float maxAngle = 180f;

    private bool isDoorOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CanDoorOpen(minAngle, maxAngle))
        {
            if (!isDoorOpen)
            {
                transform.Rotate(Vector3.up, -90f);
                isDoorOpen = true;
            }
        }
        else
        {
            if (isDoorOpen)
            {
                transform.Rotate(Vector3.up, 90f);
                isDoorOpen = false;
            }
        }
    }

    bool CanDoorOpen(float minAngle, float maxAngle)
    {
        // check if the door can be opened
        foreach (Transform controller in clockControllers)
        {
            float angle = controller.localEulerAngles.z;
            if (angle >= minAngle && angle <= maxAngle)
            {
                Debug.Log("Door is open!");
                return true;
            }
        }
        return false;
    }
}
