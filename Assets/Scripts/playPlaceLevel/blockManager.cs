using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockManager : MonoBehaviour
{
    public List<GameObject> blocks;
    public string pickupableTag = "Pickupable";
    public string tempTag = "Interactable";
    private static bool lightShowActive = false;

    
    public void BlockClicked(GameObject block)
    {
        if (!PlayPlaceManager.Instance.IsClockInteracted() && !lightShowActive)
        {
            StartCoroutine(LightShowWithDelay());
            // play lightshow music
            AkSoundEngine.PostEvent("Play_Lv1_LightShowMusic", this.gameObject);
        }
        else if (PlayPlaceManager.Instance.IsClockInteracted() && !lightShowActive && block.tag != "attached")
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
