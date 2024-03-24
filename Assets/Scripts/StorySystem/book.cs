using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private float pageSpeed = 0.5f;
    [SerializeField] private List<Transform> pages;
    private int currentPageIndex = 0;
    private bool isRotating = false;
    private bool isJournalOpen = false;
    private GameObject playerBody;
    private fpsCameraControl cameraControlScript; // Assuming this is the name of the script controlling the camera

    void Start()
    {
        isJournalOpen = false;
        playerBody = GameObject.Find("Player");
        if (Camera.main != null)
        {
            cameraControlScript = Camera.main.GetComponent<fpsCameraControl>();
        }
        ToggleJournalDisplay(isJournalOpen);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleJournal(isJournalOpen);
        }

        if (isJournalOpen)
        {
            if (!isRotating)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) && currentPageIndex < pages.Count - 1)
                {
                    StartCoroutine(RotatePage(currentPageIndex, 180));
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentPageIndex > 0)
                {
                    StartCoroutine(RotatePage(currentPageIndex - 1, 0));
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleJournal(isJournalOpen);
            }
        }
    }

    private void ToggleJournal(bool open)
    {
        isJournalOpen = !open;
        ToggleJournalDisplay(isJournalOpen);
        playerBody.GetComponent<playerMovement>().enabled = !isJournalOpen;
        LockCameraRotation(isJournalOpen);
    }

    private void LockCameraRotation(bool lockRotation)
    {
        if (cameraControlScript != null)
        {
            cameraControlScript.enabled = !lockRotation;
        }
    }

    private IEnumerator RotatePage(int index, float targetAngle)
    {
        isRotating = true;
        Quaternion startRotation = pages[index].rotation;
        Quaternion endRotation = Quaternion.Euler(0, targetAngle, 0);
        float rotationProgress = 0f;

        while (rotationProgress < 1f)
        {
            rotationProgress += Time.unscaledDeltaTime * pageSpeed;
            pages[index].rotation = Quaternion.Slerp(startRotation, endRotation, rotationProgress);
            yield return null;
        }

        currentPageIndex += (targetAngle == 180) ? 1 : -1;
        isRotating = false;
    }

    private void ToggleJournalDisplay(bool show)
    {
        Transform journalUI = transform.GetChild(0);
        journalUI.gameObject.SetActive(show);
    }
}