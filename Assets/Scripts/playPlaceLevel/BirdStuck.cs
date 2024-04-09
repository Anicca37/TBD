using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdStuck : MonoBehaviour, IInteract
{
    [SerializeField] private VoiceLine stuck;

    public void OnMouseDown()
    {
        Debug.Log("Chirp!");
        GameObject theBird = GameObject.Find("smallBird");
        AkSoundEngine.PostEvent("Play_Birds", theBird.gameObject);
        VoiceLineManager.Instance.PlayVoiceLine(stuck);
    }
}