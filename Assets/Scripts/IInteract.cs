using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    // This is a simple interface for objects that can be interacted with
    // should Implement OnMouseDown
    public void OnMouseDown()
    {
        Debug.Log("Interacted");
    }
}
