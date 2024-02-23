using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject menuSprite;
    public GameObject selectLevelController;

    public GameObject startOptionSelectedSprite;
    public GameObject levelsOptionSelectedSprite;
    public GameObject exitOptionSelectedSprite;

    private enum MenuOption { Start, Levels, Exit };
    private MenuOption selectedOption;

    private void Start()
    {
        selectLevelController.GetComponent<SelectLevelController>().enabled = false;
        // default selection
        selectedOption = MenuOption.Start;
        startOptionSelectedSprite.SetActive(true);
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
            case MenuOption.Exit:
                selectedOption = MenuOption.Levels;
                exitOptionSelectedSprite.SetActive(false);
                levelsOptionSelectedSprite.SetActive(true);
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
                selectedOption = MenuOption.Exit;
                levelsOptionSelectedSprite.SetActive(false);
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
                SceneManager.LoadScene("DemoLevel");
                break;
            case MenuOption.Levels:
                levelsOptionSelectedSprite.SetActive(false);
                // disable mainmenucontroller and enable selectlevelcontroller
                selectLevelController.GetComponent<SelectLevelController>().enabled = true;
                selectLevelController.GetComponent<SelectLevelController>().InitializeLevelSelect();
                gameObject.GetComponent<MainMenuController>().enabled = false;
                break;
            case MenuOption.Exit:
                Application.Quit();
                break;
        }
    }
}