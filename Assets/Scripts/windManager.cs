using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public Vector3 windDirection;
    public ParticleSystem windParticles;

    public void ChangeWindDirection(Vector3 newDirection)
    {
        windDirection = newDirection;
        UpdateWindParticles();
        Debug.Log("Wind direction changed to: " + windDirection);
    }

    private void UpdateWindParticles()
    {
        if (windParticles != null)
        {
            var mainModule = windParticles.main;
            mainModule.startSpeed = new ParticleSystem.MinMaxCurve(5f * windDirection.magnitude);

            // rotation angle to face the wind direction
            float angle = Mathf.Atan2(windDirection.z, windDirection.x) * Mathf.Rad2Deg;
            mainModule.startRotation = angle * Mathf.Deg2Rad; // set rotation
        }
        else
        {
            Debug.LogError("WindParticles not assigned in the WindManager.");
        }
    }


}

