using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem fountainParticleSystem;
    private bool isModified = false; // Flag to track the state of the fountain effects

    // Original settings placeholders
    private float originalStartSpeed;
    private float originalStartSize;
    private int originalMaxParticles;
    private float originalRateOverTime;
    private float originalAngle;

    void Start()
    {
        // Save original settings
        if (fountainParticleSystem != null)
        {
            var main = fountainParticleSystem.main;
            originalStartSpeed = main.startSpeed.constant;
            originalStartSize = main.startSize.constant;
            originalMaxParticles = main.maxParticles;

            var emission = fountainParticleSystem.emission;
            originalRateOverTime = emission.rateOverTime.constant;

            var shape = fountainParticleSystem.shape;
            originalAngle = shape.angle;
        }
    }

    public void ActivateFountainEffects()
    {
        if (fountainParticleSystem != null)
        {
            if (!isModified)
            {
                // Apply modified settings
                Debug.Log("Applying modified fountain effects.");

                var main = fountainParticleSystem.main;
                main.startSpeed = 10; // Modified speed
                main.startSize = 0.5f; // Modified size
                main.maxParticles = 1000; // Modified max particles

                var emission = fountainParticleSystem.emission;
                emission.rateOverTime = 300; // Modified rate

                var shape = fountainParticleSystem.shape;
                shape.angle = 25; // Modified angle
                fountainParticleSystem.Play();

                isModified = true; // Update flag to indicate modified state
            }
            else
            {
                // Revert to original settings
                Debug.Log("Reverting to original fountain effects.");

                var main = fountainParticleSystem.main;
                main.startSpeed = originalStartSpeed;
                main.startSize = originalStartSize;
                main.maxParticles = originalMaxParticles;

                var emission = fountainParticleSystem.emission;
                emission.rateOverTime = originalRateOverTime;

                var shape = fountainParticleSystem.shape;
                shape.angle = originalAngle;
                fountainParticleSystem.Play();

                isModified = false; // Update flag to indicate normal state
            }
        }
        else
        {
            Debug.LogWarning("Fountain Particle System not assigned.");
        }
    }
}
