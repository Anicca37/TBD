using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLevelController : MonoBehaviour
{
    public GameObject selectLevelSprite;
    public GameObject mainMenuController;

    public GameObject tutorialLevelSprite;
    public GameObject ballpitLevelSprite;
    public GameObject gardenLevelSprite;
    public GameObject menuSprite;

    private enum MenuOption { Tutorial, Ballpit, Garden, Menu};
    private MenuOption selectedOption;

    private void Start()
    {
        mainMenuController.GetComponent<MainMenuController>().enabled = false;
        // default selection
        selectedOption = MenuOption.Tutorial;
        tutorialLevelSprite.SetActive(true);
    }

    public void InitializeLevelSelect()
    {
        // default selection
        selectedOption = MenuOption.Tutorial;
        tutorialLevelSprite.SetActive(true);
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
            case MenuOption.Tutorial:
                selectedOption = MenuOption.Menu;
                tutorialLevelSprite.SetActive(false);
                menuSprite.SetActive(true);
                break;
            case MenuOption.Ballpit:
                selectedOption = MenuOption.Tutorial;
                ballpitLevelSprite.SetActive(false);
                tutorialLevelSprite.SetActive(true);
                break;
            case MenuOption.Garden:
                selectedOption = MenuOption.Ballpit;
                gardenLevelSprite.SetActive(false);
                ballpitLevelSprite.SetActive(true);
                break;
            case MenuOption.Menu:
                selectedOption = MenuOption.Garden;
                menuSprite.SetActive(false);
                gardenLevelSprite.SetActive(true);
                break;
        }
    }

    private void MoveSelectionDown()
    {
        switch (selectedOption)
        {
            case MenuOption.Tutorial:
                selectedOption = MenuOption.Ballpit;
                tutorialLevelSprite.SetActive(false);
                ballpitLevelSprite.SetActive(true);
                break;
            case MenuOption.Ballpit:
                selectedOption = MenuOption.Garden;
                ballpitLevelSprite.SetActive(false);
                gardenLevelSprite.SetActive(true);
                break;
            case MenuOption.Garden:
                selectedOption = MenuOption.Menu;
                gardenLevelSprite.SetActive(false);
                menuSprite.SetActive(true);
                break;
            case MenuOption.Menu:
                selectedOption = MenuOption.Tutorial;
                menuSprite.SetActive(false);
                tutorialLevelSprite.SetActive(true);
                break;
        }
    }

    private void SelectOption()
    {
        switch (selectedOption)
        {
            case MenuOption.Tutorial:
                SceneManager.LoadScene("DemoLevel");
                break;
            case MenuOption.Ballpit:
                Debug.Log("Ballpit level not implemented yet");
                // SceneManager.LoadScene("Garden_2");
                break;
            case MenuOption.Garden:
                SceneManager.LoadScene("Garden_2");
                break;
            case MenuOption.Menu:
                menuSprite.SetActive(false);
                // disable selectlevelcontroller and enable mainmenucontroller
                mainMenuController.GetComponent<MainMenuController>().enabled = true;
                mainMenuController.GetComponent<MainMenuController>().InitializeMenu();
                gameObject.GetComponent<SelectLevelController>().enabled = false;
                break;
        }
    }
}