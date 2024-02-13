using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isFloralMatched = false;
    private bool isWindChimesPlayed = false;
    private bool isClockSet = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
                    isClockSet = true;
                    MakeVenusFlytrapBloom(); // Sequence correct
                }
                break;
            case "Scales": // Scales interacted at any point floods the garden
                FloodGarden();
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

    void ResetPuzzles()
    {
        Debug.Log("Resetting puzzles.");
        isFloralMatched = false;
        isWindChimesPlayed = false;
        isClockSet = false;
        // Optionally, reload the scene to visually reset everything
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
