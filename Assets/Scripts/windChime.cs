using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windChime : MonoBehaviour
{
    public ParticleSystem windParticleSystem;

    void OnMouseDown()
    {
        switch (gameObject.name)
        {
            case "Chime1":
                ChangeWindDirection(Vector3.forward); // n
                break;
            case "Chime2":
                ChangeWindDirection(Vector3.back); // s
                break;
            case "Chime3":
                ChangeWindDirection(Vector3.right); // e
                break;
            case "Chime4":
                ChangeWindDirection(Vector3.left); // w
                break;
            default:
                break;
        }
    }

    void ChangeWindDirection(Vector3 direction)
    {
        // particle's shape
        var shape = windParticleSystem.shape;

        // convert the direction to a rotation
        Quaternion rotation = Quaternion.LookRotation(direction);
        shape.rotation = rotation.eulerAngles;

        Debug.Log($"Changing wind direction to {direction}");
    }

}