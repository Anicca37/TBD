using UnityEngine;

[RequireComponent(typeof(Light), typeof(ParticleSystem))] // Ensure there are Light and ParticleSystem components
public class InteractableGlow : MonoBehaviour
{
    private Light glowLight; // Reference to the Light component
    private ParticleSystem particles; // Reference to the ParticleSystem component
    public bool isActive = true; // Public variable to control the state, enabled by default

    // Start is called before the first frame update
    void Start()
    {


        // Configure the light
        glowLight = GetComponent<Light>();
        glowLight.color = Color.yellow;
        glowLight.intensity = 0.25f;
        glowLight.range = 10f;
        glowLight.enabled = isActive;

        // Disable shadows to prevent the object from occluding the light
        glowLight.shadows = LightShadows.None;


        // Configure the ParticleSystem
        particles = GetComponent<ParticleSystem>();

        // Set scaling mode to Local to prevent parent scaling affecting the particles
        var main = particles.main;
        main.scalingMode = ParticleSystemScalingMode.Local;


        main.startColor = new Color(1.0f, 0.84f, 0.0f); // Golden color
        main.startSize = 0.3f; // Smaller particles
        main.startSpeed = 0.5f; // Slower movement
        main.maxParticles = 70;
        main.startLifetime = 1.9f; // Shorter lifetime so they disappear after lingering

        // Adjust emission settings
        var emission = particles.emission;
        emission.rateOverTime = 5; // Lower emission rate

        // Adjust shape settings for all direction emission
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.1f; // Smaller radius for tighter emission source

        // Make particles shrink over time
        var sizeOverLifetime = particles.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1, curve);

        particles.enableEmission = isActive; // Control emission with isActive

        // Play or stop particles based on isActive
        if (isActive)
        {
            particles.Play();
        }
        else
        {
            particles.Stop();
        }
    }

    // Public method to set the isActive variable and update accordingly
    public void SetActive(bool active)
    {
        isActive = active;
        glowLight.enabled = isActive;

        if (isActive)
        {
            particles.Play();
        }
        else
        {
            particles.Stop();
        }
    }
}
