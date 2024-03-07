using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManipulation : MonoBehaviour
{
    public GameObject playerBody;
    public GameObject mainCamera;
    public GameObject clockCamera;
    private Camera currentCamera;

    public Transform[] clockHands;
    public Transform[] clockControllers;
    public float defaultHeight = 4.45f;
    private float rotationAmount = 0f;
    private Vector3 lastMousePosition;

    public Material defaultMaterial;
    public Material highlightMaterial;
    public GameObject defaultIcon;
    public GameObject grabIcon;
    public GameObject PauseMenuController;

    public Light directionalLight;
    private bool isDay = true;

    public float interactRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Stop_Clock_Tick_Reverse", this.gameObject);
        AkSoundEngine.PostEvent("Stop_Clock_Tick", this.gameObject);
        AkSoundEngine.PostEvent("Play_Clock_Tick", this.gameObject);

        currentCamera = mainCamera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LockGameControl(bool highlight)
    {
        HighlightClockHands(highlight);
        if (!PauseMenuController.GetComponent<PauseMenuController>().isGamePaused())
        {
            defaultIcon.SetActive(!highlight);
            grabIcon.SetActive(highlight);
        }
        LockPlayerMovement(highlight);
        LockCameraRotation(highlight);
    }

    void OnMouseDown()
    {
        if (CheckInteractable(playerBody.transform.position.y))
        {
            lastMousePosition = Input.mousePosition;
            LockGameControl(true);
        }
    }

    void OnMouseUp()
    {
        LockGameControl(false);
    }

    void OnMouseDrag()
    {
        if (CheckInteractable(playerBody.transform.position.y))
        {
            Vector3 center = currentCamera.WorldToScreenPoint(clockControllers[0].position);
            float anglePrevious = Mathf.Atan2(center.x - lastMousePosition.x, lastMousePosition.y - center.y);
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            float angleNow = Mathf.Atan2(center.x - (lastMousePosition.x + mouseX), (lastMousePosition.y + mouseY) - center.y);
            float dragAmount = angleNow - anglePrevious;
            lastMousePosition += new Vector3(mouseX, mouseY, 0f);
            if (dragAmount <= 6f && dragAmount >= -6f){
                // handle precision issue
                HandleClockAdjustment(dragAmount);
            }
        }
    }

    bool CheckInteractable(float playerHeight)
    {
        RaycastHit hit;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Interactable") && playerHeight >= defaultHeight)
            {
                return true;
            }
        }
        return false;
    }

    void LockPlayerMovement(bool lockMovement)
    {
        playerBody.GetComponent<playerMovement>().enabled = !lockMovement;
    }

    void LockCameraRotation(bool lockRotation)
    {
        mainCamera.SetActive(!lockRotation);
        clockCamera.SetActive(lockRotation);
        if (lockRotation)
        {
            currentCamera = clockCamera.GetComponent<Camera>();
        }
        else
        {
            currentCamera = mainCamera.GetComponent<Camera>();
        }
    }

    void HighlightClockHands(bool highlight)
    {
        //play sound
        if (highlight == true)
        {
            AkSoundEngine.PostEvent("Stop_Clock_Tick", this.gameObject);
            AkSoundEngine.PostEvent("Play_Clock_Tick_Reverse", this.gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Stop_Clock_Tick_Reverse", this.gameObject);
        }

        foreach (Transform clockHand in clockHands)
        {
            Renderer renderer = clockHand.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = highlight ? highlightMaterial : defaultMaterial;
            }
        }
    }

    void HandleClockAdjustment(float dragAmount)
    {
        // rotate the clock hand
        foreach (Transform controller in clockControllers)
        {
            controller.Rotate(new Vector3(0, 0, dragAmount * Mathf.Rad2Deg));
            // update rotation amount
            rotationAmount += dragAmount * Mathf.Rad2Deg;
            if (rotationAmount > 720f)
            {
                rotationAmount -= 720f;
            }
            else if (rotationAmount < -720f)
            {
                rotationAmount += 720f;
            }
        }

        // change the directional light rotation and color
        if (directionalLight != null)
        {
            directionalLight.transform.Rotate(new Vector3(0, dragAmount * Mathf.Rad2Deg / 2, 0), Space.Self);
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
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene.Contains("Garden"))
        {
            GardenManager.Instance.CompletePuzzle("Clock");
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
