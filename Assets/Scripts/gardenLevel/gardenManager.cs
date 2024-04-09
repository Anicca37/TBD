using System.Collections;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    public static GardenManager Instance;

    public Transform playerTransform; // Assign in inspector
    public Transform shockwaveItem; // Assign the clock object's transform in inspector


    public GameObject player;
    public GameObject VenusFlytrap;
    public ClockManipulation ClockController;
    public GameObject EscapeController;
    public Camera playerCamera;
    public Camera scalesCamera;
    public Camera birdCamera;
    public Camera venusFlytrapCamera;

    private bool isFloralMatched = false;
    private bool isWindChimesPlayed = false;
    private bool isScaleBalanced = false;
    private bool isClockSet = false;
    public bool PlayerEaten = false;

    public GameObject waterObject;
    public VineGrowthController vineGrowthController;
    private bool vineSoundPlayed = false;

    private bool isGardenFlooded = false;
    [SerializeField] private float riseSpeed = 0.15f;

    // private float floodDelay = 1.0f;
    [SerializeField] private float riseAmount = 20f;

    private float initialYPosition;

    private bool startFlood = false;
    private bool startSink = false; // New flag for controlling the sinking process

    public FountainScript fountainScript;

    public AttachPineconeToScale attachPinecone;

    [SerializeField] private Animator scaleAnimator;
    [SerializeField] private Animator birdAnimator;

    public float shockwaveCooldown = 1f; // Cooldown in seconds
    private float lastShockwaveTime = -Mathf.Infinity; // Initialize with a value that allows immediate use
    private bool StatueLoudPlayed = false;
    private bool isTrapActive = false;
    private bool isVenusFlytrapBloomed = false;
    public bool isReset = false;

    private bool isScaleBalanceSoundPlayed = false;

    [SerializeField] private VoiceLine enterGarden;
    [SerializeField] private VoiceLine balanced;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);

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

        StartCoroutine(WaitForVoiceLineManager());
    }

    IEnumerator WaitForVoiceLineManager()
    {
        // Wait until VoiceLineManager is no longer null
        yield return new WaitUntil(() => VoiceLineManager.Instance != null);
        VoiceLineManager.Instance.AssignSubtitleTextComponent();

        // Now it's safe to use VoiceLineManager.Instance
        VoiceLineManager.Instance.PlayVoiceLine(enterGarden);
    }

    public bool IsFloralMatched()
    {
        return isFloralMatched;
    }

    public bool IsWindChimesPlayed
    { get { return isWindChimesPlayed; } }

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
                    DeliverPinecone();
                }
                break;
            case "Clock":
                if (isScaleBalanced) // Clock set too early
                {
                    // Update the vine growth
                    if (vineGrowthController != null)
                    {
                        vineGrowthController.UpdateVineGrowth(ClockController.GetRotationAmount());

                        if (vineSoundPlayed == false)
                        {
                            GameObject theVines = GameObject.Find("vines");
                            AkSoundEngine.PostEvent("Play_Vine_Growing", theVines.gameObject);
                            vineSoundPlayed = true;
                        }
                    }
                    if (ClockController.CheckClockSet(1f, 180f, "Clockwise"))
                    {
                        Debug.Log("clock being set");
                        isClockSet = true;
                        MakeVenusFlytrapBloom();
                    }
                }
                else
                {
                    StatuesSingLoudly();
                }
                break;
            case "Scales":
                if (attachPinecone.isPineconeAttached)
                {
                    BalanceScales();
                    break;
                }

                FloodGarden();
                break;

            case "Escape":
                if (isClockSet)
                {
                    StartCoroutine(PlayerEnable(false, 0f));
                    EscapeGardenDelay(2f);
                }
                break;
        }
    }

    void BalanceScales()
    {
        if (!isScaleBalanced)
        {
            VoiceLineManager.Instance.PlayVoiceLine(balanced);
            scaleAnimator.SetTrigger("Balance");
            StartCoroutine(SwitchCamera(playerCamera, scalesCamera, 0f));
            StartCoroutine(PlayerEnable(false, 0f));

            if (isScaleBalanceSoundPlayed == false)
            {
                GameObject theScale = GameObject.Find("Scale");
                AkSoundEngine.PostEvent("Play_Scale_Balancing", theScale.gameObject);
                isScaleBalanceSoundPlayed = true;
            }

            isScaleBalanced = true;
            Debug.Log("Scales balanced.");
            scaleAnimator.SetTrigger("Balanced Idle");
            Invoke("BirdHint", 4.5f);
            Invoke("playBirdSound", 4.5f);
            Invoke("playBirdWingSound", 7.5f);
            StartCoroutine(SwitchCamera(birdCamera, playerCamera, 11.5f));
            Invoke("BirdIdle", 11.5f);
            StartCoroutine(PlayerEnable(true, 11.5f));
        }
    }

    void BirdHint()
    {
        birdAnimator.SetTrigger("Moove");
        StartCoroutine(SwitchCamera(scalesCamera, birdCamera, 0f));
    }

    void BirdIdle()
    {
        birdAnimator.SetTrigger("Rest");
    }
    void DirectWindToWindChimes()
    {
        Debug.Log("Wind directed to wind chimes.");
    }

    public void DeliverPinecone()
    {
        StartCoroutine(PlayerEnable(false, 1f));
        isWindChimesPlayed = true;
        Debug.Log("Bird delivers the pinecone.");
        StartCoroutine(PlayerEnable(true, 5f));
    }

    void MakeVenusFlytrapBloom()
    {
        if (isVenusFlytrapBloomed)
        {
            return;
        }
        ClockController.CancelClockPlay();
        isVenusFlytrapBloomed = true;
        Debug.Log("Venus flytrap blooms, revealing escape path.");
        VenusFlytrap.GetComponent<VenusFlytrapController>().VenusFlytrapGrow();
        if (isTrapActive == false)
        {
            // play sound
            AkSoundEngine.PostEvent("Play_FlyTrapPopedUp", VenusFlytrap.gameObject);
        }
        isTrapActive = true;
        StartCoroutine(PlayerEnable(false, 0f));
        StartCoroutine(SwitchCamera(playerCamera, venusFlytrapCamera, 0f));
        StartCoroutine(SwitchCamera(venusFlytrapCamera, playerCamera, 9f));
        StartCoroutine(PlayerEnable(true, 9f));
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
            GameObject Statue = GameObject.Find("Statue");
            // AkSoundEngine.PostEvent("Play_Statue_Loud", Statue.gameObject);
            StartCoroutine(Shockwave()); // Initiate the shockwave coroutine
            lastShockwaveTime = Time.time; // Update the last shockwave time
        }
        else
        {
            Debug.Log("Shockwave is on cooldown.");
            //StatueLoudPlayed = false;
        }
    }


    IEnumerator Shockwave()
    {
        float shockwaveDuration = 1f; // Duration of the shockwave effect
        float startTime = Time.time; // Record the start time
        float shockwaveUpwardSpeed = 10.0f; // Speed at which the player is pushed upward
        float shockwaveBackwardSpeed = -20.0f; // Speed at which the player is pushed backward

        //play sound
        if (StatueLoudPlayed == false)
        {
            GameObject Statue = GameObject.Find("Statue");
            AkSoundEngine.PostEvent("Play_Statue_Loud", Statue.gameObject);
            StatueLoudPlayed = true;
        }

        while (Time.time < startTime + shockwaveDuration)
        {
            // move the player with character controller
            CharacterController controller = playerTransform.GetComponent<CharacterController>();
            Vector3 moveDirection = playerTransform.forward * shockwaveBackwardSpeed + Vector3.up * shockwaveUpwardSpeed;
            controller.Move(moveDirection * Time.deltaTime);

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
                    StartCoroutine(WaitBeforeSink(3f));
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
    IEnumerator WaitBeforeSink(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        startSink = true;
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

    private void EscapeGardenDelay(float delay)
    {
        EscapeMenuController.ReserveEscape();
        Invoke("EscapeGarden", delay);
    }

    void EscapeGarden()
    {
        if (PlayerEaten == false)
        {
            // play sound
            AkSoundEngine.PostEvent("Play_PlayerEaten", this.gameObject);

            AkSoundEngine.PostEvent("Play_Win", this.gameObject);
        }
        PlayerEaten = true;
        EscapeController.GetComponent<EscapeMenuController>().OnEscapeActivated();
    }

    IEnumerator SwitchCamera(Camera cameraToDisable, Camera cameraToEnable, float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraToDisable.gameObject.SetActive(false);
        cameraToEnable.gameObject.SetActive(true);

    }

    IEnumerator PlayerEnable(bool enable, float delay)
    {
        yield return new WaitForSeconds(delay);
        player.GetComponent<playerMovement>().enabled = enable;
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

    public bool IsScaleBalanced()
    {
        return isScaleBalanced;
    }

    private void playBirdSound()
    {
        AkSoundEngine.PostEvent("Play_Birds", this.gameObject);
    }

    private void playBirdWingSound()
    {
        AkSoundEngine.PostEvent("Play_BirdWing", this.gameObject);
    }
}
