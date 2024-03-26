using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Book : MonoBehaviour
{
    [SerializeField] private float pageSpeed = 0.5f;
    [SerializeField] private List<Transform> pages;
    private int currentPageIndex = 0;
    private bool isRotating = false;
    private bool isJournalOpen = false;
    private GameObject playerBody;
    private fpsCameraControl cameraControlScript;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        InitialState();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitialState();
    }

    public void InitialState()
    {
        isJournalOpen = false;
        currentPageIndex = 0;
        AssignPlayerReferences();
        ToggleJournalDisplay(isJournalOpen);
        CloseJournal();
    }

    private void AssignPlayerReferences()
    {
        playerBody = GameObject.FindWithTag("Player");
        if (Camera.main != null)
        {
            cameraControlScript = Camera.main.GetComponent<fpsCameraControl>();
        }
    }

    void Update()
    {
        if (InputManager.instance.BookOpenCloseInput && playerBody != null)
        {
            ToggleJournal(!isJournalOpen);
        }

        if (isJournalOpen && !isRotating)
        {
            if (InputManager.instance.SelectionRightInput && currentPageIndex < pages.Count)
            {
                StartCoroutine(RotatePage(currentPageIndex, 180));
            }
            else if (InputManager.instance.SelectionLeftInput && currentPageIndex > 0)
            {
                StartCoroutine(RotatePage(currentPageIndex - 1, 0));
            }
        }
    }

    public void ToggleJournal(bool open)
    {
        isJournalOpen = open;
        ToggleJournalDisplay(isJournalOpen);
        if (playerBody != null)
        {
            playerBody.GetComponent<playerMovement>().enabled = !isJournalOpen;
        }
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

        pages[index].SetAsLastSibling();
        while (rotationProgress < 1f)
        {
            rotationProgress += Time.unscaledDeltaTime * pageSpeed;
            pages[index].rotation = Quaternion.Slerp(startRotation, endRotation, rotationProgress);
            yield return null;
        }

        currentPageIndex = (targetAngle == 180) ? index + 1 : index;
        isRotating = false;
    }

    private void ToggleJournalDisplay(bool show)
    {
        Transform journalUI = transform.GetChild(0);
        journalUI.gameObject.SetActive(show);
    }

    public void CloseJournal()
    {
        foreach (var page in pages)
        {
            page.rotation = Quaternion.identity;
        }
        if (pages.Count > 0)
        {
            pages[0].SetAsLastSibling();
        }
    }

    public void UpdatePageSprites(GameObject inactive, GameObject active)
    {
        if (inactive != null)
        {
            inactive.SetActive(false); // Hide the inactive sprite
        }
        
        if (active != null)
        {
            active.SetActive(true); // Show the active sprite
        }
    }

    public void FliptoBallPit()
    {
        var i = 0;
        while(i <= 2)
        {
            pages[i].SetAsLastSibling();
            pages[i].rotation = Quaternion.Euler(0, 180, 0);
            i+=1;
        }
    }
}
