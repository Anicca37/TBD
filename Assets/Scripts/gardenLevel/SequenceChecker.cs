using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceChecker : MonoBehaviour
{
    private int[] targetSequence = { 2, 4, 1, 3 };
    private int currentSequenceIndex = 0;
    private bool solved = false;
    public static SequenceChecker Instance;
    private int wrongAttempts = 0;
    public bool ifCorrectSoundPlayed = false;
    public Camera playerCamera;
    public Camera actionCamera;
    public Animator birdAnimator;
    [SerializeField] private Animator textAnimator;
    public static bool CanClickChime = true;

    public void ChimeClicked(int chimeID)
    {
        Debug.Log(chimeID);
        if (targetSequence[currentSequenceIndex] == chimeID && !solved)
        {
            currentSequenceIndex++;
            if (currentSequenceIndex >= targetSequence.Length)
            {
                // Debug.Log("Whoosh!");
                DeliverPinecone();
                solved = true;
                currentSequenceIndex = 0; // reset sequence after success

                GameObject theBird = GameObject.Find("smallBird");
                AkSoundEngine.PostEvent("Play_Birds", theBird.gameObject);
                Invoke("playBirdWing", 1.4f);
            }
        }
        else
        {
            currentSequenceIndex = 0; // reset if wrong is clicked
            wrongAttempts++;
            Debug.Log($"Wrong attempt #{wrongAttempts}");
            CanClickChime = false; // disable clicking
                                   //play sound
                                   // if (ifCorrectSoundPlayed == false)
                                   // {
                                   //     ifCorrectSoundPlayed = true;

            Invoke("playWrongSound", 0.3f);
            Invoke("playCorrectSound", 1.3f); // play correct after 0.5s;
            //     Invoke("makePlayedFalse", 5f);
            // }
        }
    }
    // public bool IsCorrectSequencePlayed()
    // {
    //     return ifCorrectSoundPlayed;
    // }
    private void playWrongSound()
    {
        AkSoundEngine.PostEvent("Play_WrongSequence", this.gameObject);
    }

    // private void makePlayedTrue()
    // {
    //     ifCorrectSoundPlayed = true;
    // }
    // private void makePlayedFalse()
    // {
    //     ifCorrectSoundPlayed = false;
    // }

    private void playCorrectSound()
    {
        AkSoundEngine.PostEvent("Play_Chime_Melody", this.gameObject);
        if (wrongAttempts >= 5)
        {
            textAnimator.SetTrigger("Hint C");
            Invoke("resetHintAnimation", 2.75f);
        }
        Invoke("EnableChimeClicking", 2.75f);
    }
    void resetHintAnimation()
    {
        textAnimator.SetTrigger("Hint Return C");
    }

    private void playBirdWing()
    {
        AkSoundEngine.PostEvent("Play_BirdWing", this.gameObject);
    }

    public void DeliverPinecone()
    {
        // bird animation
        birdAnimator.SetTrigger("Deliver");

        // switch camera
        StartCoroutine(SwitchCamera(playerCamera, actionCamera, 0.5f));

        StartCoroutine(SwitchCamera(actionCamera, playerCamera, 5.5f));
        Invoke("ReturnBird", 5f);
        GardenManager.Instance.CompletePuzzle("WindChimes");
    }

    void ReturnBird()
    {
        birdAnimator.SetTrigger("Return");
    }

    IEnumerator SwitchCamera(Camera cameraToDisable, Camera cameraToEnable, float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraToDisable.gameObject.SetActive(false);
        cameraToEnable.gameObject.SetActive(true);

    }

    void EnableChimeClicking()
    {
        CanClickChime = true;
    }
}
