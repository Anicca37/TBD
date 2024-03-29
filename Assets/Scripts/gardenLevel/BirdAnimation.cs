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
        GameObject theBird = GameObject.Find("smallBird");
        AkSoundEngine.PostEvent("Play_BirdWing", theBird.gameObject);
    }


    public void DisconnectJoint()
    {
        if (jointToModify != null)
        {
            pinecone.tag = "Pickupable";
            jointToModify.connectedBody = null;
        }

        Rigidbody pineconeRb = pinecone.GetComponent<Rigidbody>();
        pineconeRb.drag = 0.3f;
        
    }
}
