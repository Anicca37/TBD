using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject selectLevelController;
    public GameObject optionsMenuController;

    public GameObject startOptionSelectedSprite;
    public GameObject levelsOptionSelectedSprite;
    public GameObject optionsOptionSelectedSprite;
    public GameObject exitOptionSelectedSprite;

    private enum MenuOption { Start, Levels, Options, Exit };
    private MenuOption selectedOption;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        selectLevelController.GetComponent<SelectLevelController>().enabled = false;
        optionsMenuController.GetComponent<OptionsMenuController>().enabled = false;
        InitializeMenu();

        //play music
        AkSoundEngine.PostEvent("Play_MainMenu", this.gameObject);
    }

    public void InitializeMenu()
    {
        // default selection
        selectedOption = MenuOption.Start;
        startOptionSelectedSprite.SetActive(true);
    }

    private void Update()
    {
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

    private void MoveSelectionUp()
    {
        switch (selectedOption)
        {
            case MenuOption.Start:
                selectedOption = MenuOption.Exit;
                startOptionSelectedSprite.SetActive(false);
                exitOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Levels:
                selectedOption = MenuOption.Start;
                levelsOptionSelectedSprite.SetActive(false);
                startOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Options:
                selectedOption = MenuOption.Levels;
                optionsOptionSelectedSprite.SetActive(false);
                levelsOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Exit:
                selectedOption = MenuOption.Options;
                exitOptionSelectedSprite.SetActive(false);
                optionsOptionSelectedSprite.SetActive(true);
                break;
        }
    }

    private void MoveSelectionDown()
    {
        switch (selectedOption)
        {
            case MenuOption.Start:
                selectedOption = MenuOption.Levels;
                startOptionSelectedSprite.SetActive(false);
                levelsOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Levels:
                selectedOption = MenuOption.Options;
                levelsOptionSelectedSprite.SetActive(false);
                optionsOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Options:
                selectedOption = MenuOption.Exit;
                optionsOptionSelectedSprite.SetActive(false);
                exitOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Exit:
                selectedOption = MenuOption.Start;
                exitOptionSelectedSprite.SetActive(false);
                startOptionSelectedSprite.SetActive(true);
                break;
        }
    }

    private void SelectOption()
    {
        switch (selectedOption)
        {
            case MenuOption.Start:
                AkSoundEngine.PostEvent("Stop_MainMenu", this.gameObject);
                SceneManager.LoadScene("IntroCutScene");
                break;
            case MenuOption.Levels:
                levelsOptionSelectedSprite.SetActive(false);
                // disable mainmenucontroller and enable selectlevelcontroller
                selectLevelController.GetComponent<SelectLevelController>().enabled = true;
                selectLevelController.GetComponent<SelectLevelController>().InitializeLevelSelect();
                gameObject.GetComponent<MainMenuController>().enabled = false;
                break;
            case MenuOption.Options:
                optionsOptionSelectedSprite.SetActive(false);
                // disable mainmenucontroller and enable optionsmenucontroller
                optionsMenuController.GetComponent<OptionsMenuController>().enabled = true;
                optionsMenuController.GetComponent<OptionsMenuController>().InitializeOptionsMenu();
                gameObject.GetComponent<MainMenuController>().enabled = false;
                break;
            case MenuOption.Exit:
                Application.Quit();
                break;
        }
    }
}