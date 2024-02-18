using System.Collections;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    public static GardenManager Instance;

    public GameObject VenusFlytrap;
    public GameObject EscapeCanvas;
    public ClockManipulation clockController;
    
    private bool isFloralMatched = false;
    private bool isWindChimesPlayed = false;
    private bool isClockSet = false;


    public GameObject floorObject; // Assign your garden floor in the inspector
    private GameObject waterObject;

    [SerializeField] private ParticleSystem fountainParticleSystem;
    public Material waterMaterial; // Assign a blue water-like material in the inspector

    private bool isGardenFlooded = false;
    [SerializeField] private float riseSpeed = 0.5f;
    private float floodDelay = 1.0f; // Time to wait before checking the rise
    [SerializeField] private float riseAmount = 10f; // Amount to rise
    private float initialYPosition; // Starting Y position of the water
    private bool startFlood = false;

    



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);

            // set VenusFlytrap to inactive
            VenusFlytrap.SetActive(false);
            EscapeCanvas.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompletePuzzle(string puzzleName)
    {
        switch (puzzleName)
        {
            case "Floral":
                isFloralMatched = true;
                DirectWindToWindChimes();
                break;
            case "WindChimes":
                if (!isFloralMatched) // Wind Chimes played before Floral Matching
                {
                    AttractBirds();
                }
                else
                {
                    isWindChimesPlayed = true;
                    BlowSeedsOntoScales();
                }
                break;
            case "Clock":
                if (!isFloralMatched || !isWindChimesPlayed) // Clock set too early
                {
                    StatuesSingLoudly();
                }
                else
                {
                    if (clockController.CheckClockSet(0f, 180f))
                    {
                        isClockSet = true;
                        MakeVenusFlytrapBloom(); // Sequence correct
                    }
                }
                break;
            case "Scales": // Scales interacted at any point floods the garden
                FloodGarden();
                break;
            case "Escape":
                if (isFloralMatched && isWindChimesPlayed && isClockSet)
                {
                    EscapeGarden();
                }
                break;
        }
    }

    void DirectWindToWindChimes()
    {
        Debug.Log("Wind directed to Wind Chimes.");
    }

    void BlowSeedsOntoScales()
    {
        Debug.Log("Seeds blown onto scales, balancing them.");
    }

    void MakeVenusFlytrapBloom()
    {
        Debug.Log("Venus flytrap blooms, revealing escape path.");
        
        // set VenusFlytrap to active
        VenusFlytrap.SetActive(true);
    }

    void AttractBirds()
    {
        Debug.Log("Birds scatter seeds, causing overgrowth.");
        ResetPuzzles();
    }

    void StatuesSingLoudly()
    {
        Debug.Log("Statues sing loudly.");
        ResetPuzzles(); 
    }

     void FloodGarden()
    {
        if (!isGardenFlooded)
        {
            Debug.Log("Fountain floods the garden, reset required.");
            isGardenFlooded = true;
            AdjustFountainParticles();
            CreateAndRiseWater();
        }
    }


    void CreateAndRiseWater()
    {
        waterObject = Instantiate(floorObject, floorObject.transform.position, Quaternion.identity);
        waterObject.transform.localScale *= 1.05f;
        waterObject.GetComponent<Renderer>().material = waterMaterial;
        if (waterObject.GetComponent<Collider>())
            Destroy(waterObject.GetComponent<Collider>());
        
        initialYPosition = waterObject.transform.position.y; // Record the starting Y position
    }

    void Update()
{
    if (isGardenFlooded && waterObject != null && !startFlood)
    {
        // Calculate the target position based on the desired rise amount
        float targetYPosition = initialYPosition + riseAmount;
        
        // Calculate the new Y position for this frame, ensuring we don't exceed the target position
        float newYPosition = Mathf.Min(waterObject.transform.position.y + (riseSpeed * Time.deltaTime), targetYPosition);
        
        // Apply the new Y position
        waterObject.transform.position = new Vector3(waterObject.transform.position.x, newYPosition, waterObject.transform.position.z);
        
        // Check if we've reached or exceeded the target rise amount
        if (newYPosition >= targetYPosition)
        {
            startFlood = true; // Mark the flooding as complete
            Invoke("ResetPuzzles", floodDelay); // Schedule the reset after a brief delay
        }
    }
}


    void AdjustFountainParticles()
    {
        if (fountainParticleSystem != null)
        {
            Debug.Log("fountain being modified");

            var main = fountainParticleSystem.main;
            main.startSpeed = 20; // Increase for faster particles
            main.startSize = 1.5f; // Increase for bigger particles
            main.maxParticles = 1000; // Increase for more particles

            var emission = fountainParticleSystem.emission;
            emission.rateOverTime = 500; // Increase for a denser stream

            var shape = fountainParticleSystem.shape;
            shape.angle = 25; // Increase for a wider output
            fountainParticleSystem.Play();
        }
        else
        {
            Debug.LogWarning("Fountain Particle System not assigned.");
        }
    }

    void EscapeGarden()
    {
        Debug.Log("Escaping the garden.");

        // set EscapeCanvas to active
        EscapeCanvas.SetActive(true);
    }

    void ResetPuzzles()
    {
        Debug.Log("Resetting puzzles.");
        isFloralMatched = false;
        isWindChimesPlayed = false;
        isClockSet = false;

        VenusFlytrap.SetActive(false);
        EscapeCanvas.SetActive(false);
        
        // Optionally, reload the scene to visually reset everything
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
