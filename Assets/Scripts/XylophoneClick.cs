using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XylophoneClick : MonoBehaviour
{
    public int xyloID;

    private void OnMouseDown()
    {
        FindObjectOfType<SequenceChecker>().XyloClicked(xyloID);
    }
}
