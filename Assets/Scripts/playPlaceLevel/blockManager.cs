using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockManager : MonoBehaviour
{
    public List<GameObject> blocks;
    public string pickupableTag = "Pickupable";
    public string tempTag = "Interactable";
    private static bool lightShowActive = false;
    [SerializeField] private VoiceLine lightshow;
    [SerializeField] private VoiceLine lightshow1;
    private int interactionCount = 0;

    
    public void BlockClicked(GameObject block)
    {
        if (!PlayPlaceManager.Instance.IsClockInteracted() && !lightShowActive)
        {
            StartCoroutine(LightShowWithDelay());

            // Play corresponding voice line based on the number of interactions
            if (interactionCount == 0)
            {
                VoiceLineManager.Instance.PlayVoiceLine(lightshow);
            }
            else if (interactionCount == 1)
            {
                VoiceLineManager.Instance.PlayVoiceLine(lightshow1);
            }
            interactionCount++; 

            // play lightshow music
            AkSoundEngine.PostEvent("Play_Lv1_LightShowMusic", this.gameObject);
        }
        else if (PlayPlaceManager.Instance.IsClockInteracted() && !lightShowActive)
        {
            block.tag = pickupableTag;
        }
    }

    IEnumerator LightShowWithDelay()
    {
        Debug.Log("Block interacted too early, initiating light show.");
        lightShowActive = true;
        ColorCycleLightShow.Instance.StartLightShow();

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);        

        ColorCycleLightShow.Instance.StopLightShow();
        lightShowActive = false;
    }

    public static bool GetLightShowStatus()
    {
        return lightShowActive;
    }
}
