using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject resumeOptionSelectedSprite;
    public GameObject restartOptionSelectedSprite;
    public GameObject optionsOptionSelectedSprite;
    public GameObject menuOptionSelectedSprite;

    private static bool isPaused = false;
    public GameObject Crosshair;
    public GameObject HandGrab;
    private GameObject playerBody;
    private Book book;
    public GameObject optionMenuController;

    private enum MenuOption { Resume, Restart, Options, Menu };
    private MenuOption selectedOption;

    private void Start()
    {
        playerBody = GameObject.Find("Player");
        book = GameObject.Find("Journal").GetComponent<Book>();
        book.enabled = true;
        isPaused = false;
        if (optionMenuController != null)
        {
            optionMenuController.GetComponent<OptionsMenuController>().enabled = false;
        }
    }

    public void InitializePauseMenu()
    {
        Crosshair.SetActive(false);
        HandGrab.SetActive(false);
        // default selection
        selectedOption = MenuOption.Resume;
        resumeOptionSelectedSprite.SetActive(true);
    }

    public static bool isGamePaused()
    {
        return isPaused;
    }

    private void LockCameraRotation(bool lockRotation)
    {
        if (Camera.main == null)
        {
            return;
        }
        Camera.main.GetComponent<fpsCameraControl>().enabled = !lockRotation;
    }

    private void Update()
    {
        if (InputManager.instance.PauseMenuOpenCloseInput && !EscapeMenuController.isPlayerEscaped() && !isPaused)
        {
            isPaused = true;
            InitializePauseMenu();
        }
        if (isPaused)
        {
            playerBody.GetComponent<playerMovement>().enabled = false;
            playerBody.GetComponent<playerPickup>().DropObject();
            Crosshair.SetActive(false);
            HandGrab.SetActive(false);
            LockCameraRotation(true);
            book.enabled = false;
            // handle input
            if (InputManager.instance.SelectionUpInput)
            {
                MoveSelectionUp();
            }
            else if (InputManager.instance.SelectionDownInput)
            {
                MoveSelectionDown();
            }
            else if (InputManager.instance.ConfirmInput)
            {
                SelectOption();
            }
        }
    }

    private void MoveSelectionUp()
    {
        switch (selectedOption)
        {
            case MenuOption.Resume:
                selectedOption = MenuOption.Menu;
                resumeOptionSelectedSprite.SetActive(false);
                menuOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Restart:
                selectedOption = MenuOption.Resume;
                restartOptionSelectedSprite.SetActive(false);
                resumeOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Options:
                selectedOption = MenuOption.Restart;
                optionsOptionSelectedSprite.SetActive(false);
                restartOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Menu:
                selectedOption = MenuOption.Options;
                menuOptionSelectedSprite.SetActive(false);
                optionsOptionSelectedSprite.SetActive(true);
                break;
        }
    }

    private void MoveSelectionDown()
    {
        switch (selectedOption)
        {
            case MenuOption.Resume:
                selectedOption = MenuOption.Restart;
                resumeOptionSelectedSprite.SetActive(false);
                restartOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Restart:
                selectedOption = MenuOption.Options;
                restartOptionSelectedSprite.SetActive(false);
                optionsOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Options:
                selectedOption = MenuOption.Menu;
                optionsOptionSelectedSprite.SetActive(false);
                menuOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Menu:
                selectedOption = MenuOption.Resume;
                menuOptionSelectedSprite.SetActive(false);
                resumeOptionSelectedSprite.SetActive(true);
                break;
        }
    }

    private void SelectOption()
    {
        switch (selectedOption)
        {
            case MenuOption.Resume:
                isPaused = false;
                resumeOptionSelectedSprite.SetActive(false);
                playerBody.GetComponent<playerMovement>().enabled = true;
                Crosshair.SetActive(true);
                LockCameraRotation(false);
                book.enabled = true;
                break;
            case MenuOption.Restart:
                if (SceneManager.GetActiveScene().name.Contains("Garden"))
                {
                    GardenManager.Instance.ResetPuzzles();
                }
                else if (SceneManager.GetActiveScene().name == "DemoLevel")
                {
                    TutorialManager.Instance.ResetPuzzles();
                }
                else if (SceneManager.GetActiveScene().name == "PlayPlace")
                {
                    PlayPlaceManager.Instance.ResetPuzzles();
                }
                break;
            case MenuOption.Options:
                optionsOptionSelectedSprite.SetActive(false);
                // disable pausemenucontroller and enable optionsmenucontroller
                optionMenuController.GetComponent<OptionsMenuController>().enabled = true;
                optionMenuController.GetComponent<OptionsMenuController>().InitializeOptionsMenu();
                gameObject.GetComponent<PauseMenuController>().enabled = false;
                break;
            case MenuOption.Menu:
                SceneManager.LoadScene("UI");
                break;
        }
    }
}
