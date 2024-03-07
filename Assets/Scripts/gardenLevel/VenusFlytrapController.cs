using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenusFlytrapController : MonoBehaviour
{
    private bool PlayerEaten = false; //For OneTimeSound


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the Venus Flytrap");
            GardenManager.Instance.CompletePuzzle("Escape");

            if (PlayerEaten == false)
            {
                // play sound
                AkSoundEngine.PostEvent("Play_PlayerEaten", this.gameObject);
            }

            PlayerEaten = true;
        }
    }
}
