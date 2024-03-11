using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorCycleLightShow : MonoBehaviour
{
    public static ColorCycleLightShow Instance;
    public Light directionalLight; 
    public float duration = 5f; // How long the light show lasts
    public Color[] colors; // Colors to cycle through

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    
    public void StartLightShow()
    {
        StartCoroutine(LightShowRoutine());
    }

    IEnumerator LightShowRoutine()
    {
        float timer = 0f;
        int colorIndex = 0;
        directionalLight.enabled = true;

        while (timer < duration)
        {
            // Cycle through colors
            directionalLight.color = colors[colorIndex % colors.Length];
            colorIndex++;

            // Wait a little before changing colors to make it a rapid color change effect
            yield return new WaitForSeconds(0.1f);

            timer += Time.deltaTime;
        }
    }
}
