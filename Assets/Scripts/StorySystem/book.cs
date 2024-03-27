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
    private GameObject crosshair;
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
        crosshair = GameObject.Find("Crosshair");
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
        crosshair.SetActive(!isJournalOpen);
        ToggleJournalDisplay(isJournalOpen);
        if (playerBody != null)
        {
            playerBody.GetComponent<playerMovement>().enabled = !isJournalOpen;
        }
        LockCameraRotation(isJournalOpen);

        if (isJournalOpen)
        {
            // play sound
            AkSoundEngine.PostEvent("Play_BookOpen", this.gameObject);
        }
        else
        {
            // play sound
            AkSoundEngine.PostEvent("Play_BookClose", this.gameObject);
        }
    }

    private void LockCameraRotation(bool lockRotation)
    {
        if (cameraControlScript != null)
        {
            cameraControlScript.enabled = !lockRotation;
        }
    }

    private IEnumerator RotatePage(int index, float targetAngle, bool updateIndex = true)
    {
        isRotating = true;
        Quaternion startRotation = pages[index].rotation;
        Quaternion endRotation = Quaternion.Euler(0, targetAngle, 0);
        float rotationProgress = 0f;


        //play sound
        AkSoundEngine.PostEvent("Play_PageTurn", this.gameObject);

        pages[index].SetAsLastSibling();
        while (rotationProgress < 1f)
        {
            rotationProgress += Time.unscaledDeltaTime * pageSpeed;
            pages[index].rotation = Quaternion.Slerp(startRotation, endRotation, rotationProgress);
            yield return null;
        }

        if (updateIndex)
        {
            currentPageIndex = (targetAngle == 180) ? index + 1 : index;
        }
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

    public void OpenPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < pages.Count)
        {
            ToggleJournal(true);
            StartCoroutine(FlipToPage(pageIndex));
        }
    }

    private IEnumerator FlipToPage(int targetIndex)
    {
        int direction = targetIndex > currentPageIndex ? 1 : -1;
        int flipCount = Mathf.Abs(currentPageIndex - targetIndex);

        for (int i = 0; i < flipCount; i++)
        {
            int pageToFlip = currentPageIndex + (direction > 0 ? 0 : -1);
            float targetAngle = direction > 0 ? 180 : 0;
            yield return RotatePage(pageToFlip, targetAngle, false);

            currentPageIndex += direction;
        }
    }

}
