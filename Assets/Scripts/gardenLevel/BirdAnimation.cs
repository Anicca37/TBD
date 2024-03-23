using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimation : MonoBehaviour
{
    public CharacterJoint jointToModify;
    public GameObject pinecone;

    void OnMouseDown()
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
