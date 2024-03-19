using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XSequenceChecker : MonoBehaviour
{
    private int[] targetSequence = {1, 3, 2, 4};
    private int currentSequenceIndex = 0;

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
            }
        }
        else
        {
            PlayPlaceManager.Instance.CompletePuzzle("Xylophone");

        }
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
