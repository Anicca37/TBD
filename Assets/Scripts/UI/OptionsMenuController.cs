using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenuController : MonoBehaviour
{
    public GameObject mainMenuController;

    public GameObject MounseSenstivityOptionSelectedSprite;
    public GameObject MusicOptionSelectedSprite;
    public GameObject SFXOptionSelectedSprite;
    public GameObject AmbienceOptionSelectedSprite;
    public GameObject ApplyOptionSelectedSprite;
    public GameObject BackOptionSelectedSprite;

    private enum MenuOption { MounseSenstivity, Music, SFX, Ambience, Apply, Back };
    private MenuOption selectedOption;

    // Start is called before the first frame update
    private void Start()
    {
        if (mainMenuController != null)
        {
            mainMenuController.GetComponent<MainMenuController>().enabled = false;
        }
        // default selection
        selectedOption = MenuOption.MounseSenstivity;
        MounseSenstivityOptionSelectedSprite.SetActive(true);
    }

    public void InitializeOptionsMenu()
    {
        // default selection
        selectedOption = MenuOption.MounseSenstivity;
        MounseSenstivityOptionSelectedSprite.SetActive(true);
    }

    // Update is called once per frame
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
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSelectionLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSelectionRight();
        }
    }

    private void MoveSelectionUp()
    {
        switch (selectedOption)
        {
            case MenuOption.MounseSenstivity:
                selectedOption = MenuOption.Apply;
                MounseSenstivityOptionSelectedSprite.SetActive(false);
                ApplyOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Music:
                selectedOption = MenuOption.MounseSenstivity;
                MusicOptionSelectedSprite.SetActive(false);
                MounseSenstivityOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.SFX:
                selectedOption = MenuOption.Music;
                SFXOptionSelectedSprite.SetActive(false);
                MusicOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Ambience:
                selectedOption = MenuOption.SFX;
                AmbienceOptionSelectedSprite.SetActive(false);
                SFXOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Apply:
                selectedOption = MenuOption.Ambience;
                ApplyOptionSelectedSprite.SetActive(false);
                AmbienceOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Back:
                selectedOption = MenuOption.Ambience;
                BackOptionSelectedSprite.SetActive(false);
                AmbienceOptionSelectedSprite.SetActive(true);
                break;
        }
    }

    private void MoveSelectionDown()
    {
        switch (selectedOption)
        {
            case MenuOption.MounseSenstivity:
                selectedOption = MenuOption.Music;
                MounseSenstivityOptionSelectedSprite.SetActive(false);
                MusicOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Music:
                selectedOption = MenuOption.SFX;
                MusicOptionSelectedSprite.SetActive(false);
                SFXOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.SFX:
                selectedOption = MenuOption.Ambience;
                SFXOptionSelectedSprite.SetActive(false);
                AmbienceOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Ambience:
                selectedOption = MenuOption.Apply;
                AmbienceOptionSelectedSprite.SetActive(false);
                ApplyOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Apply:
                break;
            case MenuOption.Back:
                break;
        }
    }

    private void MoveSelectionLeft()
    {
        switch (selectedOption)
        {
            case MenuOption.MounseSenstivity:
                break;
            case MenuOption.Music:
                break;
            case MenuOption.SFX:
                break;
            case MenuOption.Ambience:
                break;
            case MenuOption.Apply:
                break;
            case MenuOption.Back:
                selectedOption = MenuOption.Apply;
                BackOptionSelectedSprite.SetActive(false);
                ApplyOptionSelectedSprite.SetActive(true);
                break;
        }
    }

    private void MoveSelectionRight()
    {
        switch (selectedOption)
        {
            case MenuOption.MounseSenstivity:
                break;
            case MenuOption.Music:
                break;
            case MenuOption.SFX:
                break;
            case MenuOption.Ambience:
                break;
            case MenuOption.Apply:
                selectedOption = MenuOption.Back;
                ApplyOptionSelectedSprite.SetActive(false);
                BackOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Back:
                break;
        }
    }

    private void SelectOption()
    {
        switch (selectedOption)
        {
            case MenuOption.MounseSenstivity:
                break;
            case MenuOption.Music:
                break;
            case MenuOption.SFX:
                break;
            case MenuOption.Ambience:
                break;
            case MenuOption.Apply:
                // apply changes
                break;
            case MenuOption.Back:
                // back to main menu or pause menu
                if (SceneManager.GetActiveScene().name == "UI")
                {
                    BackOptionSelectedSprite.SetActive(false);
                    // disable optionsmenucontroller amd enable mainmenucontroller
                    mainMenuController.GetComponent<MainMenuController>().enabled = true;
                    mainMenuController.GetComponent<MainMenuController>().InitializeMenu();
                    gameObject.GetComponent<OptionsMenuController>().enabled = false;
                }
                else
                {
                    // disable optionsmenucontroller and enable pausemenucontroller
                }
                break;
        }
    }
}
