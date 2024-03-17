using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPlaceLightController : MonoBehaviour
{
    private Light currentLight;
    public ClockManipulation clockController;

    private bool isPlayPlaceOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        currentLight = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanPlayPlaceOpen())
        {
            if (!isPlayPlaceOpen)
            {
                currentLight.color = Color.white;
                isPlayPlaceOpen = true;

                // Play playplace open sound
            }
        }
        else
        {
            if (isPlayPlaceOpen)
            {
                currentLight.color = Color.black;
                isPlayPlaceOpen = false;

                // Play playplace close sound
            }
        }
        PlayPlaceManager.Instance.CompletePuzzle("ClockInteraction");
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
