using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XSequenceChecker : MonoBehaviour
{
    private int[] targetSequence = {1, 3, 2, 4};
    private int currentSequenceIndex = 0;

    public bool ifXyloCorrectSoundPlayed = false;

    public void XyloClicked(int xyloID)
    {
        Debug.Log(xyloID);
        if (PlayPlaceManager.Instance.AreBlocksSorted)
        {
            if (targetSequence[currentSequenceIndex] == xyloID)
            {
                currentSequenceIndex++;
                if (currentSequenceIndex >= targetSequence.Length)
                {
                    // Debug.Log("Splat!");
                    PlayPlaceManager.Instance.CompletePuzzle("Xylophone");
                    currentSequenceIndex = 0; // reset sequence after success
                }
            }
            else
            {
                currentSequenceIndex = 0; // reset if wrong is clicked

                //play sound
                if (ifXyloCorrectSoundPlayed == false)
                {
                    ifXyloCorrectSoundPlayed = true;

                    Invoke("playWrongSound", 0.3f);
                    Invoke("playCorrectSound", 1.3f); // play correct after 0.5s;
                }

                Invoke("makeXPlayedFalse", 5f);
            }
        }
        else
        {
            PlayPlaceManager.Instance.CompletePuzzle("Xylophone");

        }
    }
    private void makeXPlayedFalse()
    {
        ifXyloCorrectSoundPlayed = false;
    }

    private void playWrongSound()
    {
        AkSoundEngine.PostEvent("Play_WrongSequence", this.gameObject);
    }

    public bool IsXyloCorrectSequencePlayed()
    {
        return ifXyloCorrectSoundPlayed;
    }

    private void playCorrectSound()
    {
        GameObject theXylo = GameObject.Find("Xylo");
        AkSoundEngine.PostEvent("Play_XyloSequence", theXylo.gameObject);
    }

    // private void DropKetchup()
    // {
    //     ketchupToDrop.SetActive(true);
    //     Rigidbody rb = ketchupToDrop.GetComponent<Rigidbody>();
    //     if (rb != null)
    //     {
    //         rb.isKinematic = false;
    //     }
    // }
}
