using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainScript : MonoBehaviour
{

    [SerializeField] private ParticleSystem fountainParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateFountainEffects()
    {
        if (fountainParticleSystem != null)
        {
            Debug.Log("Fountain being modified");

            var main = fountainParticleSystem.main;
            main.startSpeed = 10; // Increase for faster particles
            main.startSize = 0.5f; // Increase for bigger particles
            main.maxParticles = 1000; // Increase for more particles

            var emission = fountainParticleSystem.emission;
            emission.rateOverTime = 300; // Increase for a denser stream

            var shape = fountainParticleSystem.shape;
            shape.angle = 25; // Increase for a wider output
            fountainParticleSystem.Play();
        }
        else
        {
            Debug.LogWarning("Fountain Particle System not assigned.");
        }
    }
}
