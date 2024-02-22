using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManipulation : MonoBehaviour
{
    public Transform playerBody;
    public Transform[] clockHands;
    public Transform[] clockControllers;
    public float defaultHeight = 4.45f;
    public float rotationSpeed = 20f;
    private float rotationAmount = 0f;

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
        CheckInteractable();

        if (playerHeight >= defaultHeight && canInteract)
        {
            if (Input.GetButtonDown("Fire2"))
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

    void CheckInteractable()
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
            playerBody.GetComponent<playerMovement>().enabled = false;
        }
        else
        {
            playerBody.GetComponent<playerMovement>().enabled = true;
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

                // update rotation amount
                rotationAmount += scrollInput * rotationSpeed;
                if (rotationAmount > 720f)
                {
                    rotationAmount -= 720f;
                }
                else if (rotationAmount < -720f)
                {
                    rotationAmount += 720f;
                }
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

    public bool CheckClockSet(float minAngle, float maxAngle, string clockwise)
    {
        // check if the clock hand is set to the correct time
        foreach (Transform controller in clockControllers)
        {
            float angle = controller.localEulerAngles.z;
            bool isClockSet = false;

            if (clockwise == "Either")
            {
                isClockSet = angle > minAngle && angle < maxAngle;
            }
            else if (clockwise == "Clockwise")
            {
                isClockSet = rotationAmount < 0 && angle > minAngle && angle < maxAngle;
            }
            else if (clockwise == "CounterClockwise")
            {
                isClockSet = rotationAmount > 0 && angle > minAngle && angle < maxAngle;
            }

            if (isClockSet)
            {
                Debug.Log("Clock is set!");
                return true;
            }
        }
        return false;
    }
}
