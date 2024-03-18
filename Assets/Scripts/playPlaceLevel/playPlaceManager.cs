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
    private Vector3 ballsinitialPosition;
    public GameObject balls;
    public GameObject ketchupToDrop;

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
        ballsinitialPosition = balls.transform.position;
    }

    public void CompletePuzzle(string puzzleName)
    {
        switch (puzzleName)
        {
            case "ClockInteraction":
                isClockInteracted = playPlaceLightController.IsPlayPlaceOpen();
                break;
            case "BlockSorting":
                if (!isClockInteracted) // Ball sorting done too early
                {
                    CauseLightShow();
                }
                else
                {
                    areBlocksSorted = true;
                    RevealXylophoneSequence();
                }
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

    void CauseLightShow()
    {
        Debug.Log("Blocks sorted too early, initiating light show.");
        StartCoroutine(LightShowWithDelay());
    }

    IEnumerator LightShowWithDelay()
    {
        ColorCycleLightShow.Instance.StartLightShow();

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        ColorCycleLightShow.Instance.StopLightShow();
    }

    void RevealXylophoneSequence()
    {
        Debug.Log("Balls sorted, revealing xylophone sequence.");
        // Insert logic to reveal the xylophone sequence here
    }
    public bool AreBlocksSorted
    {
        get { return areBlocksSorted; }
    }

    void TriggerBallAvalanche()
    {
        Debug.Log("Xylophone played too early, triggering ball avalanche.");
        balls.SetActive(true);

        StartCoroutine(DisableAndResetAfterDelay(balls, 10f));
    }
    IEnumerator DisableAndResetAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false); // disable ball
        obj.transform.position = ballsinitialPosition; // reset to original position
    }
    void DropKetchupOntoScale()
    {
        Debug.Log("Xylophone sequence correct, dropping ketchup onto scale.");
        ketchupToDrop.SetActive(true);
        Rigidbody rb = ketchupToDrop.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
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
