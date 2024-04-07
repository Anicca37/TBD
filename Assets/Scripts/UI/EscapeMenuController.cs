using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscapeMenuController : MonoBehaviour
{
    public GameObject restartOptionSelectedSprite;
    public GameObject nextOptionSelectedSprite;
    public GameObject menuOptionSelectedSprite;

    private static bool isEscaped = false;
    public GameObject Crosshair;
    public GameObject HandGrab;
    private GameObject playerBody;
    private Book book;

    private enum MenuOption { Restart, Next, Menu };
    private MenuOption selectedOption;

    private void Start()
    {
        playerBody = GameObject.Find("Player");
        GameObject journalObject = GameObject.Find("Journal");

        if (journalObject != null)
        {
            book = journalObject.GetComponent<Book>();
            if (book != null)
            {
                book.enabled = true;
            }
        }

        isEscaped = false;
    }

    public void InitializeEscapeMenu()
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

    public static void ReserveEscape()
    {
        isEscaped = true;
    }

    public void OnEscapeActivated()
    {
        if (!isEscaped)
        {
            isEscaped = true;
        }
        if (SceneManager.GetActiveScene().name == "Garden_3 - Terrain")
        {
            StartCoroutine(DelayedLoadGardenEndScene());
        }
        else if (SceneManager.GetActiveScene().name == "PlayPlace Remap")
        {
            SceneManager.LoadScene("PlayPlaceEnd");
        }
        else 
        {
            InitializeEscapeMenu();
        }
    }

    IEnumerator DelayedLoadGardenEndScene()
    {
        yield return new WaitForSeconds(2.5f); 
        SceneManager.LoadScene("GardenEnd");
    }

    private void Update()
    {
        if (isEscaped)
        {
            if (playerBody != null){
                playerBody.GetComponent<playerMovement>().enabled = false;
            }
            
            if (book != null)
            {
                book.enabled = true;
            }
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
                if (SceneManager.GetActiveScene().name.Contains("Garden"))
                {
                    SceneManager.LoadScene("GardenIntro");
                }
                else if (SceneManager.GetActiveScene().name == "DemoLevel")
                {
                    SceneManager.LoadScene("IntroCutScene");
                }
                else if (SceneManager.GetActiveScene().name.Contains("PlayPlace"))
                {
                    SceneManager.LoadScene("PlayPlaceIntro");
                }
                break;
            case MenuOption.Next:
                if (SceneManager.GetActiveScene().name == "DemoLevel")
                {
                   SceneManager.LoadScene("PlayPlaceIntro");
                }
                else if (SceneManager.GetActiveScene().name.Contains("PlayPlace"))
                {
                    SceneManager.LoadScene("GardenIntro");
                }
                else if (SceneManager.GetActiveScene().name.Contains("Garden"))
                {
                    SceneManager.LoadScene("UI");
                }              
                break;
            case MenuOption.Menu:
                SceneManager.LoadScene("UI");
                break;
        }
    }
}
