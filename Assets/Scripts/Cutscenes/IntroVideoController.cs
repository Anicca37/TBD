using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; 
    public string nextSceneName = "DemoLevel"; 

    void Start()
    {
        // Make sure the video player component is attached
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
        
        // Subscribe to the loopPointReached event which is called when the video finishes
        videoPlayer.loopPointReached += EndReached;
    
    }

    void Update()
    {
        // Check for the Enter key to skip the video
        if (InputManager.instance.ConfirmInput)
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
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
