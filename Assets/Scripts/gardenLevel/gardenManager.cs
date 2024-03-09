using System.Collections;
using System.Collections;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    public static GardenManager Instance;

    public GameObject VenusFlytrap;
    public ClockManipulation ClockController;
    public GameObject EscapeController;

    private bool isFloralMatched = false;
    private bool isWindChimesPlayed = false;
    private bool isScaleBalanced = false;
    private bool isClockSet = false;

    public GameObject waterObject;

    private bool isGardenFlooded = false;
    [SerializeField] private float riseSpeed = 0.5f;
    private float floodDelay = 1.0f; // Time to wait before checking the rise
    [SerializeField] private float riseAmount = 10f; // Amount to rise
    private float initialYPosition; // Starting Y position of the water
    private bool startFlood = false;

    public FountainScript fountainScript;

    public GameObject scaleBeam;


    private bool StatueLoudPlayed = false;
    private bool isTrapActive = false;
    public bool isReset = false;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);

            // set VenusFlytrap to inactive
            VenusFlytrap.SetActive(false);

            //Play BGM
            AkSoundEngine.PostEvent("Play_Level2_GardenMusic", this.gameObject);
            AkSoundEngine.PostEvent("Stop_Clock_Tick_Reverse", ClockController.gameObject);
            GameObject Fountain = GameObject.Find("Fountain");
            AkSoundEngine.PostEvent("Play_Waterflow", Fountain.gameObject);
            GameObject GardenAmbience = GameObject.Find("GardenAmbience");
            AkSoundEngine.PostEvent("Play_Level2_GardenAmbience", GardenAmbience.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public bool IsFloralMatched()
    {
        return isFloralMatched;
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
                if (!isFloralMatched || !isWindChimesPlayed || !isScaleBalanced) // Clock set too early
                {
                    StatuesSingLoudly();
                }
                else
                {
                    if (ClockController.CheckClockSet(1f, 180f, "Clockwise"))
                    {
                        isClockSet = true;
                        MakeVenusFlytrapBloom(); // Sequence correct
                    }
                }
                break;
            case "Scales":
                if (isWindChimesPlayed)
                {
                    BalanceScales();
                    break;
                }

                FloodGarden(); 
                break;

            case "Escape":
                if (isFloralMatched && isWindChimesPlayed && isClockSet && isScaleBalanced)
                {
                    EscapeGarden();
                }
                break;
        }
    }

    void BalanceScales()
    {
        scaleBeam.transform.eulerAngles = new Vector3(0, 0, 0);
        isScaleBalanced = true;
        Debug.Log("Scales balanced.");
    }

    void DirectWindToWindChimes()
    {
        Debug.Log("Wind directed to Wind Chimes.");
    }

    public void BlowSeedsOntoScales()
    {
        isWindChimesPlayed = true;
        Debug.Log("Seeds blown onto scales, balancing them.");
    }

    void MakeVenusFlytrapBloom()
    {
        Debug.Log("Venus flytrap blooms, revealing escape path.");

        // set VenusFlytrap to active
        VenusFlytrap.SetActive(true);

        if (isTrapActive == false)
        {
            // play sound
            AkSoundEngine.PostEvent("Play_FlyTrapPopedUp", VenusFlytrap.gameObject);
        }

        isTrapActive = true;
    }

    void AttractBirds()
    {
        Debug.Log("Birds scatter seeds, causing overgrowth.");
        
        if (isReset == false)
        {
            ResetPuzzles();
        }
        isReset = true;
    }

    void StatuesSingLoudly()
    {
        Debug.Log("Statues sing loudly.");
        
        
        if (StatueLoudPlayed == false)
        {
            //play sound   
            GameObject Statue = GameObject.Find("Statue");
            AkSoundEngine.PostEvent("Play_Statue_Loud", Statue.gameObject);
            Invoke("ResetPuzzles", 1f);
        }
        StatueLoudPlayed = true;
    }

    void FloodGarden()
    {
        if (!isGardenFlooded)
        {
            Debug.Log("Fountain floods the garden, reset required.");
            isGardenFlooded = true;

            //play sound
            GameObject risingWater = GameObject.Find("risingWater");
            AkSoundEngine.PostEvent("Play_WaterFlooding", risingWater.gameObject);

            AdjustFountainParticles();
            CreateAndRiseWater();
        }
    }

    void CreateAndRiseWater()
    {
        waterObject.transform.localScale *= 1.05f;
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
        if (fountainScript != null)
        {
            fountainScript.ActivateFountainEffects();
        }
        else
        {
            Debug.LogWarning("FountainScript not assigned.");
        }
    }

    void EscapeGarden()
    {
        Debug.Log("Escaping the garden.");
        EscapeController.GetComponent<EscapeMenuController>().OnEscapeActivated();
    }

    public void ResetPuzzles()
    {
        Debug.Log("Resetting puzzles.");
        isFloralMatched = false;
        isWindChimesPlayed = false;
        isClockSet = false;

        VenusFlytrap.SetActive(false);
        ColorMatch.ResetMatchedFlowersCount();

        //Stop BGM
        AkSoundEngine.PostEvent("Stop_Level2_GardenMusic", this.gameObject);
        AkSoundEngine.PostEvent("Stop_Clock_Tick", ClockController.gameObject);
        AkSoundEngine.PostEvent("Stop_Clock_Tick_Reverse", ClockController.gameObject);

        GameObject Fountain = GameObject.Find("Fountain");
        AkSoundEngine.PostEvent("Stop_Waterflow", Fountain.gameObject);
        GameObject TheWind = GameObject.Find("wind");
        AkSoundEngine.PostEvent("Stop_Wind_Blowing", TheWind.gameObject);
        // AkSoundEngine.ExecuteActionOnEvent("Stop_Level2_GardenMusic", AkActionOnEventType.AkActionOnEventType_Stop);
        GameObject GardenAmbience = GameObject.Find("GardenAmbience");
        AkSoundEngine.PostEvent("Stop_Level2_GardenAmbience", GardenAmbience.gameObject);

        // Optionally, reload the scene to visually reset everything
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

}
