using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayPlaceManager : MonoBehaviour
{
    public static PlayPlaceManager Instance;

    private bool isClockInteracted = false;
    private bool areBlocksSorted = false;
    private bool isXylophoneSequenceCorrect = false;

    public PlayPlaceLightController playPlaceLightController;
    public GameObject EscapeController;

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
                isClockInteracted = playPlaceLightController.IsPlayPlaceOpen();
                break;
            case "BlockSorting":
                areBlocksSorted = true;
                RevealXylophoneSequence(); 
                break;
            case "Xylophone":
                if (!areBlocksSorted) // Xylophone played too early
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
                if (isClockInteracted && areBlocksSorted && isXylophoneSequenceCorrect)
                {
                    EscapePlayPlace();
                }
                break;
        }
    }

    public bool IsClockInteracted()
    {
        return isClockInteracted;
    }

    void HighlightBlocks()
    {
        Debug.Log("Clock interacted, highlighting balls.");
        // Insert logic to highlight balls here
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
        EscapeController.GetComponent<EscapeMenuController>().OnEscapeActivated();
    }

    public void ResetPuzzles()
    {   
        // Stop music here

        isClockInteracted = false;
        areBlocksSorted = false;
        isXylophoneSequenceCorrect = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
