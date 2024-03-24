using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimation : MonoBehaviour, IInteract
{
    public CharacterJoint jointToModify;
    public GameObject pinecone;

    public void OnMouseDown()
    {
        Debug.Log("Chirp!");
        // ketro add chirp here!!!!
    }


    public void DisconnectJoint()
    {
        if (jointToModify != null)
        {
            pinecone.tag = "Pickupable";
            jointToModify.connectedBody = null;
        }
    }
}
