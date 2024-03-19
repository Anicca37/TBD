using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenuController : MonoBehaviour
{
    public GameObject mainMenuController;
    public GameObject pauseMenuController;

    public GameObject MounseSenstivityOptionSelectedSprite;
    public GameObject MusicOptionSelectedSprite;
    public GameObject SFXOptionSelectedSprite;
    public GameObject AmbienceOptionSelectedSprite;
    public GameObject ApplyOptionSelectedSprite;
    public GameObject BackOptionSelectedSprite;

    public GameObject MounseSenstivitySlider;
    public GameObject MusicSlider;
    public GameObject SFXSlider;
    public GameObject AmbienceSlider;

    private float currentMouseSensitivity = 3f;
    private float currentMusicVolume = 50f;
    private float currentSfxVolume = 50f;
    private float currentAmbienceVolume = 50f;
    private float newMouseSensitivity = 3f;
    private float newMusicVolume = 50f;
    private float newSfxVolume = 50f;
    private float newAmbienceVolume = 50f;

    private enum MenuOption { MounseSenstivity, Music, SFX, Ambience, Apply, Back };
    private MenuOption selectedOption;

    // Start is called before the first frame update
    private void Start()
    {
        if (mainMenuController != null)
        {
            mainMenuController.GetComponent<MainMenuController>().enabled = false;
        }
        if (pauseMenuController != null)
        {
            pauseMenuController.GetComponent<PauseMenuController>().enabled = false;
        }
        InitializeOptionsMenu();
    }

    public void InitializeOptionsMenu()
    {
        // default selection
        selectedOption = MenuOption.MounseSenstivity;
        MounseSenstivityOptionSelectedSprite.SetActive(true);
        MounseSenstivitySlider.SetActive(true);
        MusicSlider.SetActive(true);
        SFXSlider.SetActive(true);
        AmbienceSlider.SetActive(true);
        // load saved settings
        currentMouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", currentMouseSensitivity);
        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", currentMusicVolume);
        currentSfxVolume = PlayerPrefs.GetFloat("SFXVolume", currentSfxVolume);
        currentAmbienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", currentAmbienceVolume);
        // set new settings to current settings
        newMouseSensitivity = currentMouseSensitivity;
        newMusicVolume = currentMusicVolume;
        newSfxVolume = currentSfxVolume;
        newAmbienceVolume = currentAmbienceVolume;
        // set sliders to saved settings
        SetSliderValue(MounseSenstivitySlider, currentMouseSensitivity);
        SetSliderValue(MusicSlider, currentMusicVolume);
        SetSliderValue(SFXSlider, currentSfxVolume);
        SetSliderValue(AmbienceSlider, currentAmbienceVolume);
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

    private float IncreaseSliderValue(GameObject currentSlider)
    {
        float currentValue = currentSlider.GetComponent<Slider>().value;
        float newValue = currentValue;
        if (currentSlider.name == "MouseSensSlider")
        {
            newValue += 1f; // Increase value by 1
        }
        else
        {
            newValue += 10f; // Increase value by 10
        }
        
        newValue =  Mathf.Clamp(newValue, 
                                currentSlider.GetComponent<Slider>().minValue, 
                                currentSlider.GetComponent<Slider>().maxValue);
        currentSlider.GetComponent<Slider>().value = newValue;
        return newValue;
    }

    private float DecreaseSliderValue(GameObject currentSlider)
    {
        float currentValue = currentSlider.GetComponent<Slider>().value;
        float newValue = currentValue;
        if (currentSlider.name == "MouseSensSlider")
        {
            newValue -= 1f; // Decrease value by 1
        }
        else
        {
            newValue -= 10f; // Decrease value by 10
        }
        newValue =  Mathf.Clamp(newValue, 
                                currentSlider.GetComponent<Slider>().minValue, 
                                currentSlider.GetComponent<Slider>().maxValue);
        currentSlider.GetComponent<Slider>().value = newValue;
        return newValue;
    }

    private void SetSliderValue(GameObject currentSlider, float value)
    {
        currentSlider.GetComponent<Slider>().value = value;
    }

    private void MoveSelectionLeft()
    {
        switch (selectedOption)
        {
            case MenuOption.MounseSenstivity:
                newMouseSensitivity = DecreaseSliderValue(MounseSenstivitySlider);
                break;
            case MenuOption.Music:
                newMusicVolume = DecreaseSliderValue(MusicSlider);
                break;
            case MenuOption.SFX:
                newSfxVolume = DecreaseSliderValue(SFXSlider);
                break;
            case MenuOption.Ambience:
                newAmbienceVolume = DecreaseSliderValue(AmbienceSlider);
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
                newMouseSensitivity = IncreaseSliderValue(MounseSenstivitySlider);
                break;
            case MenuOption.Music:
                newMusicVolume = IncreaseSliderValue(MusicSlider);
                break;
            case MenuOption.SFX:
                newSfxVolume = IncreaseSliderValue(SFXSlider);
                break;
            case MenuOption.Ambience:
                newAmbienceVolume = IncreaseSliderValue(AmbienceSlider);
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
                currentMouseSensitivity = newMouseSensitivity;
                currentMusicVolume = newMusicVolume;
                currentSfxVolume = newSfxVolume;
                currentAmbienceVolume = newAmbienceVolume;
                // save changes
                PlayerPrefs.SetFloat("MouseSensitivity", currentMouseSensitivity);
                PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
                PlayerPrefs.SetFloat("SFXVolume", currentSfxVolume);
                PlayerPrefs.SetFloat("AmbienceVolume", currentAmbienceVolume);
                break;
            case MenuOption.Back:
                SetSliderValue(MounseSenstivitySlider, currentMouseSensitivity);
                SetSliderValue(MusicSlider, currentMusicVolume);
                SetSliderValue(SFXSlider, currentSfxVolume);
                SetSliderValue(AmbienceSlider, currentAmbienceVolume);
                // back to main menu or pause menu
                if (SceneManager.GetActiveScene().name == "UI")
                {
                    if (mainMenuController == null)
                    {
                        return;
                    }
                    BackOptionSelectedSprite.SetActive(false);
                    MounseSenstivitySlider.SetActive(false);
                    MusicSlider.SetActive(false);
                    SFXSlider.SetActive(false);
                    AmbienceSlider.SetActive(false);
                    // disable optionsmenucontroller amd enable mainmenucontroller
                    mainMenuController.GetComponent<MainMenuController>().enabled = true;
                    mainMenuController.GetComponent<MainMenuController>().InitializeMenu();
                    gameObject.GetComponent<OptionsMenuController>().enabled = false;
                }
                else
                {
                    if (pauseMenuController == null)
                    {
                        return;
                    }
                    BackOptionSelectedSprite.SetActive(false);
                    MounseSenstivitySlider.SetActive(false);
                    MusicSlider.SetActive(false);
                    SFXSlider.SetActive(false);
                    AmbienceSlider.SetActive(false);
                    // disable optionsmenucontroller and enable pausemenucontroller
                    pauseMenuController.GetComponent<PauseMenuController>().enabled = true;
                    pauseMenuController.GetComponent<PauseMenuController>().InitializePauseMenu();
                    gameObject.GetComponent<OptionsMenuController>().enabled = false;
                }
                break;
        }
    }
}
