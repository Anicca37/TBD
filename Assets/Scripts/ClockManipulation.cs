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
    public GameObject defaultIcon;
    public GameObject grabIcon;

    public Light directionalLight;

    public float interactRange = 10f;
    private bool canInteract = false;
    private bool isHighlighted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player can reach the clock
        float playerHeight = playerBody.position.y;
        checkInteractable();

        if (playerHeight >= chairHeight && canInteract)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                HighlightClockHands(!isHighlighted);
                defaultIcon.SetActive(!isHighlighted);
                grabIcon.SetActive(isHighlighted);
                LockPlayerMovement(isHighlighted);
            }
        }
        else
        {
            HighlightClockHands(false);
            defaultIcon.SetActive(true);
            grabIcon.SetActive(false);
            LockPlayerMovement(false);
        }
        if (isHighlighted)
        {
            HandleClockAdjustment();
        }

    }

    void checkInteractable()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                canInteract = true;
            }
        }
        else
        {
            canInteract = false;
        }
    }

    void LockPlayerMovement(bool lockMovement)
    {
        if (lockMovement)
        {
            playerBody.GetComponent<CharacterController>().enabled = false;
        }
        else
        {
            playerBody.GetComponent<CharacterController>().enabled = true;
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
