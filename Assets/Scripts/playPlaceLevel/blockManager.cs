using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockManager : MonoBehaviour
{
    public List<GameObject> blocks;
    public string pickupableTag = "Pickupable";
    public string tempTag = "TempUntagged";
    private static bool lightShowActive = false;

    
    public void BlockClicked(GameObject block)
    {
        if (!PlayPlaceManager.Instance.IsClockInteracted() && !lightShowActive)
        {
            StartCoroutine(LightShowWithDelay());
            // play lightshow music
            AkSoundEngine.PostEvent("Play_Lv1_LightShowMusic", this.gameObject);
        }
    }

    IEnumerator LightShowWithDelay()
    {
        Debug.Log("Block interacted too early, initiating light show.");
        lightShowActive = true;
        ColorCycleLightShow.Instance.StartLightShow();
        TagAllBlocks(tempTag); // Temporarily untag all blocks to prevent interaction

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);        

        TagAllBlocks(pickupableTag); // Restore the pickupable tag to all blocks
        ColorCycleLightShow.Instance.StopLightShow();
        lightShowActive = false;
    }

    void TagAllBlocks(string newTag)
    {
        foreach (var block in blocks)
        {
            block.tag = newTag;
        }
    }

    public static bool GetLightShowStatus()
    {
        return lightShowActive;
    }
}
