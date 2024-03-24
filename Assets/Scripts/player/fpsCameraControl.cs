using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpsCameraControl : MonoBehaviour
{
    private float mouseSensitivity = 300f;

    public Transform player;

    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 3f) * 10f;

        Vector2 looking = FPSInputManager.GetPlayerLook();

        float mouseX = looking.x * mouseSensitivity * Time.deltaTime;
        float mouseY = looking.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}
