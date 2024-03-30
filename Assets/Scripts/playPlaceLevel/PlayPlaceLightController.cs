using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayPlaceLightController : MonoBehaviour
{
    private Light currentLight;
    public GameObject[] flouresLights;
    public GameObject[] cameras;
    public ClockManipulation clockController;
    public GameObject openSign;
    public GameObject openSignOn;

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
        PlayPlaceManager.Instance.CompletePuzzle("ClockInteraction");
        GameObject theCelling = GameObject.Find("LightSoundEmitter");
        if (CanPlayPlaceOpen())
        {
            if (!isPlayPlaceOpen)
            {
                isPlayPlaceOpen = true;
                BlinkAndToggle(isPlayPlaceOpen);
                Invoke("DelayedPlayPlaceOperations", 1f);

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
                isPlayPlaceOpen = false;
                BlinkAndToggle(isPlayPlaceOpen);
                Invoke("DelayedPlayPlaceOperations", 1f);

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
    }
    
    private void DelayedPlayPlaceOperations()
    {
        PlayPlaceOperations(isPlayPlaceOpen);
    }

    private void PlayPlaceOperations(bool isOpen)
    {
        CameraEffect(isOpen);
        currentLight.color = isOpen ? Color.white : Color.black;
        currentLight.intensity = 1f;
        openSign.SetActive(!isOpen);
        openSignOn.SetActive(isOpen);
    }

    private void BlinkAndToggle(bool isOpen)
    {
        StartCoroutine(BlinkCoroutine(isOpen));
    }

    IEnumerator BlinkCoroutine(bool isOpen)
    {
        int blinkCount = 7;
        float blinkInterval = 0.1f;
        bool isLightOn = false;

        for (int i = 0; i < blinkCount; i++)
        {
            foreach (GameObject light in flouresLights)
            {
                light.SetActive(!isLightOn);
            }
            isLightOn = !isLightOn;
            if (i != blinkCount - 1)
            {
                yield return new WaitForSeconds(blinkInterval);
            }
        }

        foreach (GameObject light in flouresLights)
        {
            light.SetActive(false);
        }
    }

    private void CameraEffect(bool isOpen)
    {
        foreach (GameObject camera in cameras)
        {
            camera.GetComponent<PostProcessLayer>().enabled = !isOpen;
            camera.GetComponent<PostProcessVolume>().enabled = !isOpen;
        }
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
        if ((rotationAmount > 90f && rotationAmount < 270f) || 
            (rotationAmount > -630f && rotationAmount < -450f))
        {
            return true;
        }
        
        return false;
    }
}
