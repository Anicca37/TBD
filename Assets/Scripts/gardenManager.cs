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
    private float riseSpeed = 0.5f;
    private float targetHeight = 5f;
    private bool isGardenFlooded = false;


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
    if (!isGardenFlooded) // Check if the garden isn't already flooded
    {
        Debug.Log("Fountain floods the garden, reset required.");
        isGardenFlooded = true; // Prevent multiple floods
        CreateAndRiseWater();
        // Moved ResetPuzzles call to the end of the RiseWater coroutine
    }
}

    void CreateAndRiseWater()
{
    // Duplicate the floor object and modify it to become the water object
    waterObject = Instantiate(floorObject, floorObject.transform.position, Quaternion.identity);
    waterObject.transform.localScale = new Vector3(waterObject.transform.localScale.x * 1.05f, waterObject.transform.localScale.y, waterObject.transform.localScale.z * 1.05f); // Make the water object slightly larger
    waterObject.GetComponent<Renderer>().material = waterMaterial; // Change the material to water
    waterObject.transform.position -= new Vector3(0, 0.5f, 0); // Start below the floor to rise up

    // Optionally, disable or remove unnecessary components (e.g., colliders) from the water object
    if (waterObject.GetComponent<Collider>())
    {
        Destroy(waterObject.GetComponent<Collider>()); // Assuming you don't need collision
    }

    StartCoroutine(RiseWater());
}


    IEnumerator RiseWater()
{
    float targetHeight = 2f; // Define how high the water should rise
    while (waterObject.transform.position.y < targetHeight)
    {
        waterObject.transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        yield return null;
    }
    // After reaching the target height, call ResetPuzzles to reset the game
    ResetPuzzles();
}

    void AdjustFountainParticles()
    {
        if (fountainParticleSystem != null)
        {
            var main = fountainParticleSystem.main;
            main.startSpeed = 20; // Increase for faster particles
            main.startSize = 1.5f; // Increase for bigger particles
            main.maxParticles = 10000; // Increase for more particles

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
