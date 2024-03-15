using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XSequenceChecker : MonoBehaviour
{
    private int[] targetSequence = { 1, 4, 2, 3 };
    private int currentSequenceIndex = 0;
    public GameObject ketchupToDrop;

    public void XyloClicked(int xyloID)
    {
        Debug.Log(xyloID);

        if (targetSequence[currentSequenceIndex] == xyloID)
        {
            currentSequenceIndex++;
            if (currentSequenceIndex >= targetSequence.Length)
            {
                Debug.Log("Splat!");
                DropKetchup();
                currentSequenceIndex = 0; // reset sequence after success
            }
        }
        else
        {
            currentSequenceIndex = 0; // reset if wrong is clicked
        }
    }
    private void DropKetchup()
    {
        Rigidbody rb = ketchupToDrop.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
