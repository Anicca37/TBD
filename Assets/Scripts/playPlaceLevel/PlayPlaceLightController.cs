using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPlaceLightController : MonoBehaviour
{
    private Light currentLight;
    public ClockManipulation clockController;

    private bool isPlayPlaceOpen = false;

    private bool isLightSoundPlayed = false;
    private bool isLightOffSoundPlayed = false;
    private bool isLvlMusicPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        currentLight = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject theCelling = GameObject.Find("LightSoundEmitter");
        if (CanPlayPlaceOpen())
        {
            if (!isPlayPlaceOpen)
            {
                currentLight.color = Color.white;
                isPlayPlaceOpen = true;

                // Play playplace open sound
                if (isLvlMusicPlayed == false)
                {
                    AkSoundEngine.PostEvent("Play_Lv1_PlayPlaceMusic", this.gameObject);
                }
                isLvlMusicPlayed = true;
                AkSoundEngine.SetRTPCValue("Lv1_LightOn", 100);

                if (isLightOffSoundPlayed == true)
                {
                    AkSoundEngine.PostEvent("Stop_LightOff_1", theCelling.gameObject);
                }
                if (isLightSoundPlayed == false)
                {
                    isLightSoundPlayed = true;
                    AkSoundEngine.PostEvent("Play_LightOnSound", theCelling.gameObject);
                    Invoke("LightSoundReset", 10f);
                }
                
            }
        }
        else
        {
            if (isPlayPlaceOpen)
            {
                currentLight.color = Color.black;
                isPlayPlaceOpen = false;

                // Play playplace close sound
                if (isLightSoundPlayed == true)
                {
                    AkSoundEngine.PostEvent("Stop_LightOnSound", theCelling.gameObject);
                }
                if (isLightOffSoundPlayed == false)
                {
                    isLightOffSoundPlayed = true;
                    AkSoundEngine.PostEvent("Play_LightOff_1", theCelling.gameObject);
                    Invoke("LightOffSoundReset", 10f);
                }
                AkSoundEngine.SetRTPCValue("Lv1_LightOn", 0);
            }
        }
        PlayPlaceManager.Instance.CompletePuzzle("ClockInteraction");
    }

    private void LightSoundReset()
    {
        isLightSoundPlayed = false;
    }
    private void LightOffSoundReset()
    {
        isLightOffSoundPlayed = false;
    }

    public bool IsPlayPlaceOpen()
    {
        return isPlayPlaceOpen;
    }

    private bool CanPlayPlaceOpen()
    {
        float rotationAmount = clockController.GetRotationAmount();
        if ((rotationAmount > 90f && rotationAmount < 450f) || 
            (rotationAmount > -630f && rotationAmount < -270f))
        {
            return true;
        }
        
        return false;
    }
}
