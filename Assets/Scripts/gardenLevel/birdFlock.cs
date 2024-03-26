using UnityEngine;

public class ParticleToGameObject : MonoBehaviour
{
    public ParticleSystem currentParticleSystem;
    public GameObject gameObjectToInstantiate;

    void Update()
    {
        // Instantiate a GameObject at the position of each particle
        if (currentParticleSystem != null && gameObjectToInstantiate != null)
        {
            // Get particles from the particle system
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[currentParticleSystem.particleCount];
            int numParticlesAlive = currentParticleSystem.GetParticles(particles);

            for (int i = 0; i < numParticlesAlive; i++)
            {
                // Instantiate GameObject at particle position
                Instantiate(gameObjectToInstantiate, particles[i].position, Quaternion.identity, transform);
            }

            // Optionally, clear the particle system to avoid repeating this process
            // currentParticleSystem.Clear();
        }
    }
}
