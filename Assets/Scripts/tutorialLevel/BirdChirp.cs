using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdChirp : MonoBehaviour
{
    public void OnMouseDown()
    {
        Debug.Log("Chirp!");
        // add chirp sound

        GameObject theBird = GameObject.Find("smallBird");
        AkSoundEngine.PostEvent("Play_Birds", theBird.gameObject);
    }
}
