using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    private GameObject playerBody;
    private GameObject mainCamera;
    private GameObject defaultIcon;
    private PauseMenuController pauseMenuController;
    private EscapeMenuController escapeMenuController;

    [SerializeField] GameObject grabIcon;
    [SerializeField] Texture2D cursor;

    private bool isPlayerLocked = false;
    private bool isCameraLocked = false;
    private bool grabIconActive = false;
    private bool defaultIconActive = true;
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
        defaultIcon = GameObject.Find("Crosshair");
        pauseMenuController = GameObject.Find("PauseMenuController").GetComponent<PauseMenuController>();
        escapeMenuController = GameObject.Find("EscapeMenuController").GetComponent<EscapeMenuController>();
    }

    public void LockGameControl(bool lockControl)
    {
        LockPlayerMovement(lockControl);
        LockCameraRotation(lockControl);
    }

    public void LockPlayerMovement(bool lockControl)
    {
        if (playerBody == null || isPlayerLocked == lockControl)
        {
            return;
        }
        isPlayerLocked = lockControl;
        playerBody.GetComponent<playerMovement>().enabled = !lockControl;
    }

    public void LockCameraRotation(bool lockControl)
    {
        if (mainCamera == null || isCameraLocked == lockControl)
        {
            return;
        }
        isCameraLocked = lockControl;
        mainCamera.GetComponent<fpsCameraControl>().enabled = !lockControl;
    }

    public void SwitchCursorIcon(bool cursorType)
    {
        // if paused or escaped, disable cursor icon
        if (pauseMenuController.isGamePaused() || escapeMenuController.isPlayerEscaped())
        {
            return;
        }
        // true for grab, false for default
        defaultIcon.SetActive(!cursorType);
        grabIcon.SetActive(cursorType);
    }

    public void DisableCursorIcon()
    {
        defaultIconActive = defaultIcon.activeSelf;
        grabIconActive = grabIcon.activeSelf;
        defaultIcon.SetActive(false);
        grabIcon.SetActive(false);
    }

    public void ResumeCursorIcon()
    {
        defaultIcon.SetActive(defaultIconActive);
        grabIcon.SetActive(grabIconActive);
    }

    public void EnableCursor(bool enable)
    {
        Cursor.visible = enable;
        Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.SetCursor(!enable ? null : cursor, Vector2.zero, CursorMode.Auto);
    }
}
