using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManipulation : MonoBehaviour
{
    public Transform playerBody;
    public Transform[] clockHands;
    public Transform[] clockControllers;
    public float chairHeight = 4.45f;
    public float rotationSpeed = 10f;

    public Material defaultMaterial;
    public Material highlightMaterial;

    public Light directionalLight;

    private bool isHighlighted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player height is above the height of the chair
        float playerHeight = playerBody.position.y;
        if (playerHeight >= chairHeight)
        {
            HighlightClockHands(true);
        }
        else
        {
            HighlightClockHands(false);
        }
        if (isHighlighted)
        {
            HandleClockAdjustment();
        }

    }

    void HighlightClockHands(bool highlight)
    {
        isHighlighted = highlight;
        foreach (Transform clockHand in clockHands)
        {
            Renderer renderer = clockHand.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = isHighlighted ? highlightMaterial : defaultMaterial;
            }
        }
    }

    void HandleClockAdjustment()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            // rotate the clock hand
            foreach (Transform controller in clockControllers)
            {
                controller.Rotate(Vector3.forward, scrollInput * rotationSpeed);
            }
            // change the light rotation and color
            if (directionalLight != null)
            {
                directionalLight.transform.Rotate(Vector3.up, scrollInput * rotationSpeed, Space.Self);
                float time = Mathf.Repeat(directionalLight.transform.rotation.eulerAngles.y, 360f) / 360f;
                directionalLight.color = Color.Lerp(Color.black, Color.white, time);
            }
            
        }
    }
}
