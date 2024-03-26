using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Book : MonoBehaviour
{
    public static Book Instance;
    [SerializeField] private List<GameObject> inactiveSpritesOnReset; 
    [SerializeField] private List<GameObject> activeSpritesOnReset;
    [SerializeField] private float pageSpeed = 0.5f;
    [SerializeField] private List<Transform> pages;
    private int currentPageIndex = 0;
    private bool isRotating = false;
    private bool isJournalOpen = false;
    private GameObject playerBody;
    private GameObject Crosshair;
    private fpsCameraControl cameraControlScript; 

    void Start()
    {
        InitialState();
    }
    
    public void InitialState()
    {
        isJournalOpen = false;
        playerBody = GameObject.Find("Player");
        Crosshair = GameObject.Find("Crosshair");
        if (Camera.main != null)
        {
            cameraControlScript = Camera.main.GetComponent<fpsCameraControl>();
        }
        ToggleJournalDisplay(isJournalOpen);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isJournalOpen = false; // Ensure the journal is closed on scene load.
        AssignPlayerReferences();
        ToggleJournalDisplay(isJournalOpen);
        if (scene.name == "DemoLevel")
        {
            ResetJournal();
        }
    }

    private void ResetJournal()
    {
        for (int i=0; i<pages.Count; i++)
        {
            pages[i].transform.rotation=Quaternion.identity;
        }
        pages[0].SetAsLastSibling();
        
        foreach (var inactiveSprite in inactiveSpritesOnReset)
        {
            inactiveSprite.SetActive(false);
            
        }

        foreach (var activeSprite in activeSpritesOnReset)
        {
           activeSprite.SetActive(true);
        }
    }

    private void AssignPlayerReferences()
    {
        playerBody = GameObject.Find("Player");
        if (Camera.main != null)
        {
            cameraControlScript = Camera.main.GetComponent<fpsCameraControl>();
        }
    }


    void Update()
    {
        if (InputManager.instance.BookOpenCloseInput && playerBody != null)
        {
            ToggleJournal(isJournalOpen);
        }

        if (!isJournalOpen)
        {
            Crosshair.SetActive(true);
        }
        
        if (isJournalOpen)
        {
            Crosshair.SetActive(false);
            if (!isRotating)
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
    }

    public void ToggleJournal(bool open)
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

        pages[index].SetAsLastSibling();
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
}
