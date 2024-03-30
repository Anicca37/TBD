using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClockManipulation : MonoBehaviour, IInteract
{
    public GameObject playerBody;
    public GameObject mainCamera;
    public GameObject clockCamera;
    private Camera currentCamera;

    public Transform[] clockHands;
    public Transform[] clockControllers;
    private float rotationAmount = 0f;
    private Vector3 lastMousePosition;
    private Vector2 lastControllerPosition;
    [SerializeField] Texture2D cursor;

    public Material defaultMaterial;
    public Material highlightMaterial;
    public GameObject defaultIcon;
    public GameObject grabIcon;

    public Light directionalLight;
    public TMP_Text clockSign;
    private bool isDay = true;
    private bool LookClock = false;

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
        if (LookClock && CheckInteractable())
        {
            // handle clock adjustment
            ControllerDrag();
            if (FPSInputManager.GetCancel())
            {
                OnMouseUp();
            }
        }
    }

    public void LockGameControl(bool highlight)
    {
        HighlightClockHands(highlight);
        if (!PauseMenuController.isGamePaused())
        {
            defaultIcon.SetActive(!highlight);
            // grabIcon.SetActive(highlight);
        }
        LockPlayerMovement(highlight);
        LockCameraRotation(highlight);
    }

    public void OnMouseDown()
    {
        if (CheckInteractable())
        {
            lastMousePosition = Input.mousePosition;
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            lastControllerPosition = new Vector2(horizontal, vertical);
            LockGameControl(true);
            LookClock = true;
        }
    }

    void OnMouseUp()
    {
        LockGameControl(false);
        LookClock = false;
    }
    
    void ControllerDrag()
    {
        // Use input get axis to get the controller input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 currentControllerPosition = new Vector2(horizontal, vertical);
        float dragAmount = Vector2.SignedAngle(lastControllerPosition, currentControllerPosition)/45f;
        lastControllerPosition = currentControllerPosition;
        if (dragAmount <= 6f && dragAmount >= -6f){
            // handle precision issue
            HandleClockAdjustment(dragAmount);
        }
    }
    
    void OnMouseDrag()
    {
        if (CheckInteractable())
        {
            Vector3 center = currentCamera.WorldToScreenPoint(clockControllers[0].position);
            float anglePrevious = Mathf.Atan2(center.x - lastMousePosition.x, lastMousePosition.y - center.y);
            Vector3 currentMousePosition = Input.mousePosition;
            float angleCurrent = Mathf.Atan2(center.x - currentMousePosition.x, currentMousePosition.y - center.y);
            float dragAmount = angleCurrent - anglePrevious;
            lastMousePosition = currentMousePosition;
            if (dragAmount <= 6f && dragAmount >= -6f){
                // handle precision issue
                HandleClockAdjustment(dragAmount);
            }
        }
    }

    bool isDemoScene()
    {
        return SceneManager.GetActiveScene().name.Contains("Demo");
    }

    bool isGardenScene()
    {
        return SceneManager.GetActiveScene().name.Contains("Garden");
    }

    bool isPlayPlaceScene()
    {
        return SceneManager.GetActiveScene().name.Contains("PlayPlace");
    }

    bool isOnChair()
    {
        RaycastHit hit;
        Transform playerTransform = playerBody.transform;
        if (Physics.Raycast(playerTransform.position, Vector3.down, out hit, 2f))
        {
            // check if hit object name is "Chair"
            if (hit.collider.gameObject.name == "Chair")
            {
                return true;
            }
        }
        return false;
    }

    bool CheckInteractable()
    {
        // if demo scene, check if player is on chair
        if (isDemoScene() && !isOnChair())
        {
            return false;
        }

        if (isGardenScene())
        {
            GardenManager.Instance.CompletePuzzle("Clock");
            if (!GardenManager.Instance.IsScaleBalanced())
            {
                return false;
            }
        }

        if (isPlayPlaceScene() && blockManager.GetLightShowStatus())
        {
            return false;
        }

        RaycastHit hit;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                return true;
            }
        }
        return false;
    }

    void EnableCursor(bool enable)
    {
        Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = enable;
        // set Cursor style to Grab Cursor Texture from Texture folder
        Cursor.SetCursor(!enable ? null : cursor, Vector2.zero, CursorMode.Auto);
    }

    void LockPlayerMovement(bool lockMovement)
    {
        playerBody.GetComponent<playerMovement>().enabled = !lockMovement;
    }

    void LockCameraRotation(bool lockRotation)
    {
        mainCamera.SetActive(!lockRotation);
        clockCamera.SetActive(lockRotation);
        EnableCursor(lockRotation);
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

        if (clockSign != null)
        {
            DisplayAMPMText(rotationAmount);
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
    }

    void DisplayAMPMText(float rotationAmount)
    {
        if ((rotationAmount < 0f && rotationAmount > -360f) ||
            (rotationAmount > 360f && rotationAmount < 720f))
        {
            clockSign.text = "AM";
        }
        else if ((rotationAmount < -360f && rotationAmount > -720f) ||
                 (rotationAmount > 0f && rotationAmount < 360f))
        {
            clockSign.text = "PM";
        }
    }

    public float GetRotationAmount()
    {
        return rotationAmount;
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
                return true;
            }
        }
        return false;
    }

    public void CancelClockPlay()
    {
        OnMouseUp();
    }
}
