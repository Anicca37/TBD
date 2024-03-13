using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceChecker : MonoBehaviour
{
    private int[] targetSequence = { 2, 4, 1, 3 };
    private int currentSequenceIndex = 0;
    public WindController windController;

    private bool solved = false;

    public static SequenceChecker Instance;
    public bool ifCorrectSoundPlayed = false;


    public void ChimeClicked(int chimeID)
    {
        Debug.Log(chimeID);

        if (targetSequence[currentSequenceIndex] == chimeID && !solved)
        {
            currentSequenceIndex++;
            if (currentSequenceIndex >= targetSequence.Length)
            {
                Debug.Log("Whoosh!");
                LaunchSeeds();
                currentSequenceIndex = 0; // reset sequence after success
                AkSoundEngine.PostEvent("Play_Birds", this.gameObject);
                Invoke("playBirdWing", 1.3f);
            }
        }
        else
        {
            currentSequenceIndex = 0; // reset if wrong is clicked
           
            //play sound
            if (ifCorrectSoundPlayed == false)
            {
                ifCorrectSoundPlayed = true;
                Invoke("playCorrectSound", 1f); // play correct after 0.5s;
            }
            
            Invoke("makePlayedFalse", 10f);
        }
    }
    public bool IsCorrectSequencePlayed()
    {
        return ifCorrectSoundPlayed;
    }

    private void makePlayedTrue()
    {
        ifCorrectSoundPlayed = true;
    }
    private void makePlayedFalse()
    {
        ifCorrectSoundPlayed = false;
    }

    private void playCorrectSound()
    {
        AkSoundEngine.PostEvent("Play_Chime_Melody", this.gameObject);
    }

    private void playBirdWing()
    {
        AkSoundEngine.PostEvent("Play_BirdWing", this.gameObject);
    }

    private void LaunchSeeds()
    {
        windController.LaunchSeedsEastward();
        solved = true;
    }
}
