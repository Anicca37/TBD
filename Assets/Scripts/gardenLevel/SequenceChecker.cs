using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceChecker : MonoBehaviour
{
    private int[] targetSequence = { 1, 4, 3, 2 };
    private int currentSequenceIndex = 0;
    public WindController windController;

    private bool solved = false;


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
            }
        }
        else
        {
            currentSequenceIndex = 0; // reset if wrong is clicked
        }
    }
    private void LaunchSeeds()
    {
        windController.LaunchSeedsEastward();
        solved = true;
    }
}
