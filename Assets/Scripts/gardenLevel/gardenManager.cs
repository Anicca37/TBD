using System.Collections;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    public static GardenManager Instance;

    public Transform playerTransform; // Assign in inspector
    public Transform shockwaveItem; // Assign the clock object's transform in inspector


    public GameObject VenusFlytrap;
    public ClockManipulation ClockController;
    public GameObject EscapeController;

    private bool isFloralMatched = false;
    private bool isWindChimesPlayed = false;
    private bool isScaleBalanced = false;
    private bool isClockSet = false;

    public GameObject waterObject;

    private bool isGardenFlooded = false;
    [SerializeField] private float riseSpeed = 0.15f;
    private float floodDelay = 1.0f; 
    [SerializeField] private float riseAmount = 10f; 
    private float initialYPosition; 
    private bool startFlood = false;
    private bool startSink = false; // New flag for controlling the sinking process

    public FountainScript fountainScript;

    public GameObject scaleBeam;


    public float shockwaveCooldown = 1f; // Cooldown in seconds
    private float lastShockwaveTime = -Mathf.Infinity; // Initialize with a value that allows immediate use
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
            AkSoundEngine.PostEvent("Play_Level2_NewGardenMusic", this.gameObject);
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
        if (Time.time >= lastShockwaveTime + shockwaveCooldown)
        {
            ClockController.LockGameControl(false);
            Debug.Log("Statues sing loudly.");
            StartCoroutine(Shockwave()); // Initiate the shockwave coroutine
            lastShockwaveTime = Time.time; // Update the last shockwave time
        }
        else
        {
            Debug.Log("Shockwave is on cooldown.");
            //StatueLoudPlayed = false;
        }


        // Debug.Log("Statues sing loudly.");
        
        
        // if (StatueLoudPlayed == false)
        // {
        //     //play sound   
        //     GameObject Statue = GameObject.Find("Statue");
        //     AkSoundEngine.PostEvent("Play_Statue_Loud", Statue.gameObject);
        //     AkSoundEngine.PostEvent("Stop_Level2_NewGardenMusic", this.gameObject);
        //     AkSoundEngine.PostEvent("Stop_Clock_Tick", ClockController.gameObject);
        //     AkSoundEngine.PostEvent("Stop_Clock_Tick_Reverse", ClockController.gameObject);
        //     GameObject Fountain = GameObject.Find("Fountain");
        //     AkSoundEngine.PostEvent("Stop_Waterflow", Fountain.gameObject);
        //     Invoke("ResetPuzzles", 1f);
        // }
        // StatueLoudPlayed = true;
    }


    IEnumerator Shockwave()
    {
        float shockwaveDuration = 1f; // Duration of the shockwave effect
        float startTime = Time.time; // Record the start time
        Vector3 originalPosition = playerTransform.position; // Record the player's original position
        Vector3 direction = (playerTransform.position - shockwaveItem.position).normalized; // Calculate the direction away from the clock
        float shockwaveSpeed = 70.0f; // Speed at which the player is pushed away

        //play sound
        if (StatueLoudPlayed == false)
        {
            GameObject Statue = GameObject.Find("Statue");
            AkSoundEngine.PostEvent("Play_Statue_Loud", Statue.gameObject);
            StatueLoudPlayed = true;
        }

        while (Time.time < startTime + shockwaveDuration)
        {
            // Calculate new position based on direction and speed
            Vector3 newPosition = playerTransform.position + direction * shockwaveSpeed * Time.deltaTime;
            // Optionally, you can include a check to ensure the player won't move through walls or other obstacles

            playerTransform.position = newPosition; // Move the player to the new position
            yield return null; // Wait until the next frame
        }

        StatueLoudPlayed = false;

        // After the shockwave, the player stops moving
        // Optionally, you can smoothly stop the player's movement by reducing the speed over time
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
    if (isGardenFlooded && waterObject != null)
    {
        if (!startFlood)
        {
            // Rising logic...
            float targetYPosition = initialYPosition + riseAmount;
            float newYPosition = Mathf.Min(waterObject.transform.position.y + (riseSpeed * Time.deltaTime), targetYPosition);
            waterObject.transform.position = new Vector3(waterObject.transform.position.x, newYPosition, waterObject.transform.position.z);

            if (newYPosition >= targetYPosition)
            {
                startFlood = true; // Mark the flooding as complete
                startSink = true; // Immediately enable sinking after reaching max height
            }
        }
        else if (startSink)
        {
            // Sinking logic
            float targetYPosition = initialYPosition;
            float newYPosition = Mathf.Max(waterObject.transform.position.y - (riseSpeed * Time.deltaTime), targetYPosition);
            waterObject.transform.position = new Vector3(waterObject.transform.position.x, newYPosition, waterObject.transform.position.z);

            if (newYPosition <= targetYPosition)
            {
                startSink = false; // Mark the sinking as complete
                isGardenFlooded = false; // Reset the flooded state to allow re-flooding
                startFlood = false; // Reset to allow flooding to start over
                initialYPosition = waterObject.transform.position.y; // Reset initial position for accurate re-flood
                AdjustFountainParticles(); // Reset or adjust visual effects as needed
            }
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
        AkSoundEngine.PostEvent("Stop_Level2_NewGardenMusic", this.gameObject);
        AkSoundEngine.PostEvent("Stop_Clock_Tick", ClockController.gameObject);
        AkSoundEngine.PostEvent("Stop_Clock_Tick_Reverse", ClockController.gameObject);

        GameObject Fountain = GameObject.Find("Fountain");
        AkSoundEngine.PostEvent("Stop_Waterflow", Fountain.gameObject);
        GameObject TheWind = GameObject.Find("wind");
        AkSoundEngine.PostEvent("Stop_Wind_Blowing", TheWind.gameObject);
        // AkSoundEngine.ExecuteActionOnEvent("Stop_Level2_NewGardenMusic", AkActionOnEventType.AkActionOnEventType_Stop);
        GameObject GardenAmbience = GameObject.Find("GardenAmbience");
        AkSoundEngine.PostEvent("Stop_Level2_GardenAmbience", GardenAmbience.gameObject);

        // Optionally, reload the scene to visually reset everything
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

}
