using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterSeeds : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public GameObject seedPrefab;

    private void Start()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        var main = particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback; // callback when the system stops
    }

    private void OnParticleSystemStopped()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
        int numParticlesAlive = particleSystem.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            Instantiate(seedPrefab, particles[i].position, Quaternion.identity);
        }
    }
}
