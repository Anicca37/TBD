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
        Debug.Log("Fountain floods the garden, reset required.");
        ResetPuzzles(); // Resets the puzzles due to flooding
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
