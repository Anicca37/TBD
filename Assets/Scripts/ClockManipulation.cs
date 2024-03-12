using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockManipulation : MonoBehaviour
{
    public GameObject playerBody;
    public GameObject mainCamera;
    public GameObject clockCamera;
    private Camera currentCamera;

    public Transform[] clockHands;
    public Transform[] clockControllers;
    private float rotationAmount = 0f;
    private Vector3 lastMousePosition;

    [SerializeField] Texture2D cursor;

    public Material defaultMaterial;
    public Material highlightMaterial;
    public GameObject defaultIcon;
    public GameObject grabIcon;
    public GameObject PauseMenuController;

    public Light directionalLight;
    private bool isDay = true;

    public float interactRange = 10f;
    public List<GameObject> vines;
    public Vector3 maxScale = new Vector3(10f, 10f, 10f);
    private Dictionary<GameObject, Vector3> originalVineScales = new Dictionary<GameObject, Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var vine in vines)
        {
            vine.transform.localScale = Vector3.zero;
            originalVineScales[vine] = maxScale;
        }

        AkSoundEngine.PostEvent("Stop_Clock_Tick_Reverse", this.gameObject);
        AkSoundEngine.PostEvent("Stop_Clock_Tick", this.gameObject);
        AkSoundEngine.PostEvent("Play_Clock_Tick", this.gameObject);

        currentCamera = mainCamera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LockGameControl(bool highlight)
    {
        HighlightClockHands(highlight);
        if (!PauseMenuController.GetComponent<PauseMenuController>().isGamePaused())
        {
            defaultIcon.SetActive(!highlight);
            // grabIcon.SetActive(highlight);
        }
        LockPlayerMovement(highlight);
        LockCameraRotation(highlight);
    }

    void OnMouseDown()
    {
        if (CheckInteractable())
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

        // Calculate growth based on clock rotation
        float growthFactor = Mathf.Clamp((Mathf.Abs(rotationAmount) % 360) / 360f, 0f, 1f);

        // Scale vines based on the growthFactor and maxScale
        foreach (var vine in vines)
        {
            vine.transform.localScale = Vector3.Lerp(Vector3.zero, originalVineScales[vine], growthFactor);
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
