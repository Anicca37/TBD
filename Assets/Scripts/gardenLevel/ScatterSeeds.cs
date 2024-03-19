using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterSeeds : MonoBehaviour
{
    public ParticleSystem currentParticleSystem;
    public GameObject seedPrefab;

    private void Start()
    {
        if (currentParticleSystem == null)
        {
            currentParticleSystem = GetComponent<ParticleSystem>();
        }

        var main = currentParticleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback; // callback when the system stops
    }

    private void OnParticleSystemStopped()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[currentParticleSystem.particleCount];
        int numParticlesAlive = currentParticleSystem.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            Instantiate(seedPrefab, particles[i].position, Quaternion.identity);
        }
    }
}
