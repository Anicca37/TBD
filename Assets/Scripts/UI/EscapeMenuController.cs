using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeMenuController : MonoBehaviour
{
    public GameObject restartOptionSelectedSprite;
    public GameObject nextOptionSelectedSprite;
    public GameObject menuOptionSelectedSprite;

    private static bool isEscaped = false;
    public GameObject Crosshair;
    public GameObject HandGrab;
    private GameObject playerBody;

    private enum MenuOption { Restart, Next, Menu };
    private MenuOption selectedOption;

    private void Start()
    {
        playerBody = GameObject.Find("Player");
    }

    private void InitializeEscapeMenu()
    {
        Crosshair.SetActive(false);
        HandGrab.SetActive(false);
        // default selection
        selectedOption = MenuOption.Next;
        nextOptionSelectedSprite.SetActive(true);
    }

    public static bool isPlayerEscaped()
    {
        return isEscaped;
    }

    public void OnEscapeActivated()
    {
        if (!isEscaped)
        {
            isEscaped = true;
            InitializeEscapeMenu();
        }
    }

    private void Update()
    {
        if (isEscaped)
        {
            playerBody.GetComponent<playerMovement>().enabled = false;
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
            case MenuOption.Restart:
                selectedOption = MenuOption.Next;
                restartOptionSelectedSprite.SetActive(false);
                nextOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Next:
                selectedOption = MenuOption.Menu;
                nextOptionSelectedSprite.SetActive(false);
                menuOptionSelectedSprite.SetActive(true);
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
            case MenuOption.Restart:
                selectedOption = MenuOption.Menu;
                restartOptionSelectedSprite.SetActive(false);
                menuOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Next:
                selectedOption = MenuOption.Restart;
                nextOptionSelectedSprite.SetActive(false);
                restartOptionSelectedSprite.SetActive(true);
                break;
            case MenuOption.Menu:
                selectedOption = MenuOption.Next;
                menuOptionSelectedSprite.SetActive(false);
                nextOptionSelectedSprite.SetActive(true);
                break;
        }
    }

    private void SelectOption()
    {
        switch (selectedOption)
        {
            case MenuOption.Restart:
                isEscaped = false;
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
            case MenuOption.Next:
                if (SceneManager.GetActiveScene().name == "Garden_3 - Terrain")
                {
                    SceneManager.LoadScene("UI");
                }
                else if (SceneManager.GetActiveScene().name == "DemoLevel")
                {
                   SceneManager.LoadScene("PlayPlace");
                }
                else if (SceneManager.GetActiveScene().name == "PlayPlace")
                {
                    SceneManager.LoadScene("Garden_3 - Terrain");
                }                
                break;
            case MenuOption.Menu:
                SceneManager.LoadScene("UI");
                break;
        }
    }
}
