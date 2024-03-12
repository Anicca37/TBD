using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseSprite;

    public GameObject resumeOptionSelectedSprite;
    public GameObject restartOptionSelectedSprite;
    public GameObject menuOptionSelectedSprite;

    private bool isPaused = false;
    private GameObject playerBody;
    private EscapeMenuController escapeMenuController;
    private ScreenController screenController;

    private enum MenuOption { Resume, Restart, Menu };
    private MenuOption selectedOption;

    private void Start()
    {
        playerBody = GameObject.Find("Player");
        escapeMenuController = GameObject.Find("EscapeMenuController").GetComponent<EscapeMenuController>();
        screenController = GameObject.Find("ScreenManager").GetComponent<ScreenController>();
    }

    private void InitializePauseMenu()
    {
        // default selection
        selectedOption = MenuOption.Resume;
        resumeOptionSelectedSprite.SetActive(true);
        screenController.DisableCursorIcon();
    }

    public bool isGamePaused()
    {
        return isPaused;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !escapeMenuController.isPlayerEscaped() && !isPaused)
        {
            isPaused = true;
            InitializePauseMenu();
        }
        if (isPaused)
        {
            screenController.LockGameControl(true);
            // handle input
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveSelectionUp();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveSelectionDown();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
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
            case MenuOption.Menu:
                selectedOption = MenuOption.Restart;
                menuOptionSelectedSprite.SetActive(false);
                restartOptionSelectedSprite.SetActive(true);
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
                selectedOption = MenuOption.Menu;
                restartOptionSelectedSprite.SetActive(false);
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
                screenController.LockGameControl(false);
                screenController.ResumeCursorIcon();
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
                break;
            case MenuOption.Menu:
                SceneManager.LoadScene("UI");
                break;
        }
    }
}
