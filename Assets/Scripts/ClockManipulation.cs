using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManipulation : MonoBehaviour
{
    public Transform playerBody;
    public Transform[] clockHands;
    public Transform[] clockControllers;
    public float chairHeight = 4.45f;
    public float rotationSpeed = 20f;

    public Material defaultMaterial;
    public Material highlightMaterial;
    public GameObject defaultIcon;
    public GameObject grabIcon;

    public Light directionalLight;
    private bool isDay = true;

    public float interactRange = 10f;
    private bool canInteract = false;
    private bool isHighlighted = false;

    private string currentScene = "";
    
    // Start is called before the first frame update
    void Start()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene: " + currentScene);
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player can reach the clock
        float playerHeight = playerBody.position.y;
        checkInteractable();

        if (playerHeight >= chairHeight && canInteract)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                HighlightClockHands(!isHighlighted);
                defaultIcon.SetActive(!isHighlighted);
                grabIcon.SetActive(isHighlighted);
                // LockPlayerMovement(isHighlighted);
            }
        }
        else
        {
            HighlightClockHands(false);
            defaultIcon.SetActive(true);
            grabIcon.SetActive(false);
            // LockPlayerMovement(false);
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
                directionalLight.transform.Rotate(Vector3.up, scrollInput * rotationSpeed / 2, Space.Self);
                float timeOfDay = Mathf.Repeat(directionalLight.transform.rotation.eulerAngles.y, 360f) / 360f;
                if (timeOfDay <= 0.5f)
                {
                    isDay = false;
                }
                else
                {
                    isDay = true;
                }

                if (isDay)
                {
                    directionalLight.color = Color.Lerp(Color.black, Color.white, (timeOfDay - 0.5f) * 2);
                }
                else
                {
                    directionalLight.color = Color.Lerp(Color.white, Color.black, timeOfDay * 2);
                }
            } 

            // check current scene
            if (currentScene == "Garden_2")
            {
                GardenManager.Instance.CompletePuzzle("Clock");  
            }
        }
    }

    public bool checkClockSet(float minAngle, float maxAngle)
    {
        // check if the clock hand is set to the correct time
        foreach (Transform controller in clockControllers)
        {
            float angle = controller.localEulerAngles.z;
            if (angle >= minAngle && angle <= maxAngle)
            {
                Debug.Log("Clock is set!");
                return true;
            }
        }
        return false;
    }
}
