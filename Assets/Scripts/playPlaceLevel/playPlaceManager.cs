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
    public Camera playerCamera;
    public Camera tunnelCamera;
    [SerializeField] private Animator doorAnimator;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameObject theClock = GameObject.Find("Clock");
            AkSoundEngine.PostEvent("Stop_Clock_Tick", theClock.gameObject);
            AkSoundEngine.PostEvent("Stop_Lv1_PlayPlaceMusic", this.gameObject);
            AkSoundEngine.PostEvent("Stop_Lv1_LightShowMusic", this.gameObject);
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
                    OpenTunnel(); // Unlock the escape mechanism
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
        
        // play sound of xylo
        GameObject theXylo = GameObject.Find("Xylo");
        AkSoundEngine.PostEvent("Play_XyloSequence", theXylo.gameObject);
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
    void OpenTunnel()
    {
        Debug.Log("Xylophone sequence correct, opening tunnel.");
        StartCoroutine(SwitchCamera(playerCamera, tunnelCamera, 0f));
        doorAnimator.SetTrigger("Open");

        //play sound        
        Invoke("playDoorSound", 1f);

        // ketchupToDrop.SetActive(true);
        // Rigidbody rb = ketchupToDrop.GetComponent<Rigidbody>();
        // if (rb != null)
        // {
        //     rb.isKinematic = false;
        // }
        StartCoroutine(SwitchCamera(tunnelCamera, playerCamera, 5f));

    }
    private void playDoorSound()
    {
        AkSoundEngine.PostEvent("Play_SlideDoorOpen_1", this.gameObject);
    }
    void EscapePlayPlace()
    {
        Debug.Log("Escaping the play place.");
        // Insert escape logic here
        EscapeController.GetComponent<EscapeMenuController>().OnEscapeActivated();

        // play sound
        AkSoundEngine.PostEvent("Play_Win", this.gameObject);
    }

    IEnumerator SwitchCamera(Camera cameraToDisable, Camera cameraToEnable, float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraToDisable.gameObject.SetActive(false);
        cameraToEnable.gameObject.SetActive(true);

    }

    public void ResetPuzzles()
    {
        // Stop music here

        isClockInteracted = false;
        areBlocksSorted = false;
        isXylophoneSequenceCorrect = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool GetIsXylophoneSequenceCorrect()
    {
        return isXylophoneSequenceCorrect;
    }
}
