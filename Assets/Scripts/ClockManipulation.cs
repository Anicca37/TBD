using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManipulation : MonoBehaviour
{
    public Transform playerBody;
    public Transform[] clockHands;
    public Transform[] clockControllers;
    public float highlightHeight = 6f;
    public float rotationSpeed = 10f;

    public Material defaultMaterial;
    public Material highlightMaterial;

    public bool isHighlighted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float playerHeight = playerBody.position.y;
        if (playerHeight >= highlightHeight)
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
            // Rotate the clock hand
            foreach (Transform controller in clockControllers)
            {
                controller.Rotate(Vector3.forward, scrollInput * rotationSpeed);
            }
            
        }
    }
}
