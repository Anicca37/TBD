using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windChime : MonoBehaviour
{
    public ParticleSystem windParticleSystem;
    public WindController windController;


    void OnMouseDown()
    {
        // Debug.Log("GM.Instance: " + GameManager.Instance);
        // check if the floral puzzle is matched

        if (true)
        // if (GameManager.Instance.IsFloralMatched())
        {
            switch (gameObject.name)
            {
                case "Chime1":
                    ChangeWindDirection(Vector3.forward); // North
                    break;
                case "Chime2":
                    ChangeWindDirection(Vector3.back); // South
                    break;
                case "Chime3":
                    ChangeWindDirection(Vector3.right); // East
                    break;
                case "Chime4":
                    ChangeWindDirection(Vector3.left); // West
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("Cool wind chimes.");
        }
    }
    void ChangeWindDirection(Vector3 direction)
    {
        var shape = windParticleSystem.shape;
        Quaternion rotation = Quaternion.LookRotation(direction);
        shape.rotation = rotation.eulerAngles;

        if (direction == Vector3.right) // East
        {
            windController.LaunchSeedsEastward();
        }

        Debug.Log($"Changing wind direction to {direction}");
    }


}