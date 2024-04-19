using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using UnityEngine.Video;
using System.Collections;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "DemoLevel";
    public GameObject loadingScreen;
    public GameObject EscapeBackground;
    public EscapeMenuController EscapeController;

    private bool IsEndReached = false;

    void Start()
    {
        videoPlayer.playOnAwake = false;
        IsEndReached = false;
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Subscribe to the loopPointReached event which is called when the video finishes
        videoPlayer.loopPointReached += EndReached;

        // Start the sequence of displaying the loading screen and then the video
        StartCoroutine(StartCutsceneAfterDelay());
    }

    IEnumerator StartCutsceneAfterDelay()
    {
        
        if(loadingScreen != null) {
            loadingScreen.SetActive(true);
            yield return new WaitForSeconds(3f);
            loadingScreen.SetActive(false);
        }
        videoPlayer.Play();

        string currentScene = SceneManager.GetActiveScene().name;
        // play sound
        if (currentScene == "IntroCutScene")
        {
            AkSoundEngine.PostEvent("Play_IntroCutscene", this.gameObject);
        } 
        else if (currentScene == "PlayPlaceIntro")
        {
            Invoke("playPlayPlaceCutSceneSound", 0.4f);
        }
        else if (currentScene == "GardenIntro")
        {
            Invoke("playGardenCutSceneSound", 0.4f);
        }
        else if (currentScene == "GardenEnd")
        {
            Invoke("playGardenEndCutSceneSound", 0.4f);
        }
        else if (currentScene == "PlayPlaceEnd")
        {
            Invoke("playBallPitEndCutScene", 0.4f);
        }

    }

    void Update()
    {
        // Check for the Enter key to skip the video
        if (InputManager.instance.ConfirmInput && !IsEndReached)
        {
            SkipVideo();

            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == "IntroCutScene")
            {
                AkSoundEngine.PostEvent("Stop_IntroCutscene", this.gameObject);
            }
            else if (currentScene == "PlayPlaceIntro")
            {
                AkSoundEngine.PostEvent("Stop_BallPitCutScene", this.gameObject);
            }
            else if (currentScene == "GardenIntro")
            {
                AkSoundEngine.PostEvent("Stop_GardenCutScene", this.gameObject);
            }
            else if (currentScene == "GardenEnd")
            {
                AkSoundEngine.PostEvent("Stop_GardenEndCutScene", this.gameObject);
            }
            else if (currentScene == "PlayPlaceEnd")
            {
                AkSoundEngine.PostEvent("Stop_BallPitEndCutScene", this.gameObject);
            }
        }
    }

    void EndReached(VideoPlayer vp)
    {
        IsEndReached = true;
        LoadNextScene();
    }

    public void SkipVideo()
    {
        videoPlayer.Stop();
        LoadNextScene();
    }

    void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().name == "PlayPlaceEnd")
        {
            StartCoroutine(DelayedEscapeActivation());
        }
        else {
            SceneManager.LoadScene(nextSceneName);
        }
    }
    
    IEnumerator DelayedEscapeActivation()
    {
        yield return new WaitForSeconds(0.5f); 
        EscapeBackground.SetActive(true);
        EscapeMenuController.ReserveEscape();
        EscapeController.InitializeEscapeMenu();
    }

    void playPlayPlaceCutSceneSound()
    {
        AkSoundEngine.PostEvent("Play_BallPitCutScene", this.gameObject);
    }

    void playGardenCutSceneSound()
    {
        AkSoundEngine.PostEvent("Play_GardenCutScene", this.gameObject);
    }

    void playGardenEndCutSceneSound()
    {
        AkSoundEngine.PostEvent("Play_GardenEndCutScene", this.gameObject);
    }

    void playBallPitEndCutScene()
    {
        AkSoundEngine.PostEvent("Play_BallPitEndCutScene", this.gameObject);
    }
}

