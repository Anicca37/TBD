using UnityEngine;

public class PlayPlaceManager : MonoBehaviour
{
    public static PlayPlaceManager Instance;

    private bool isClockInteracted = false;
    private bool areBallsSorted = false;
    private bool isXylophoneSequenceCorrect = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void CompletePuzzle(string puzzleName)
    {
        switch (puzzleName)
        {
            case "ClockInteraction":
                isClockInteracted = true;
                HighlightBalls();
                break;
            case "BallSorting":
                if (!isClockInteracted) // Ball sorting done too early
                {
                    CauseLightShow();
                }
                else
                {
                    areBallsSorted = true;
                    RevealXylophoneSequence(); 
                }
                break;
            case "Xylophone":
                if (!areBallsSorted) // Xylophone played too early
                {
                    TriggerBallAvalanche();
                }
                else
                {
                    isXylophoneSequenceCorrect = true;
                    DropKetchupOntoScale(); // Unlock the escape mechanism
                }
                break;
            case "Escape":
                if (isClockInteracted && areBallsSorted && isXylophoneSequenceCorrect)
                {
                    EscapePlayPlace();
                }
                break;
        }
    }

    void HighlightBalls()
    {
        Debug.Log("Clock interacted, highlighting balls.");
        // Insert logic to highlight balls here
    }

    void CauseLightShow()
    {
        Debug.Log("Balls sorted too early, initiating light show.");
        // Insert logic for causing a light show here
        ResetPuzzles();
    }

    void RevealXylophoneSequence()
    {
        Debug.Log("Balls sorted, revealing xylophone sequence.");
        // Insert logic to reveal the xylophone sequence here
    }

    void TriggerBallAvalanche()
    {
        Debug.Log("Xylophone played too early, triggering ball avalanche.");
        // Insert logic to trigger a ball avalanche here
        ResetPuzzles();
    }

    void DropKetchupOntoScale()
    {
        Debug.Log("Xylophone sequence correct, dropping ketchup onto scale.");
        // Insert logic to drop ketchup onto the scale here
    }

    void EscapePlayPlace()
    {
        Debug.Log("Escaping the play place.");
        // Insert escape logic here
    }


    public void ResetPuzzles()
    {
        isClockInteracted = false;
        areBallsSorted = false;
        isXylophoneSequenceCorrect = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
