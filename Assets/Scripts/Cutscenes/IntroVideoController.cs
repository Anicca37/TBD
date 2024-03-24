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

    void Start()
    {
        videoPlayer.playOnAwake = false;

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
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(3f);
        loadingScreen.SetActive(false);
        videoPlayer.Play();
    }

    void Update()
    {
        // Check for the Enter key to skip the video
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SkipVideo();
        }
    }

    void EndReached(VideoPlayer vp)
    {
        LoadNextScene();
    }

    public void SkipVideo()
    {
        videoPlayer.Stop();
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
