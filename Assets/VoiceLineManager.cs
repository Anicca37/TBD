using System.Collections;
using UnityEngine;
using TMPro;
using AK; // Make sure to include the correct Wwise namespace

public class VoiceLineManager : MonoBehaviour
{
    public static VoiceLineManager Instance { get; private set; }

    public TextMeshProUGUI subtitleText; // Assign in the inspector
    public float additionalDisplayTime = 3f; // Time to display subtitles after audio ends

    private GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    public void AssignSubtitleTextComponent()
    {
        // Find the TextMeshProUGUI component in the current scene
        // Make sure you have exactly one TextMeshProUGUI component tagged or named uniquely in your UI
        subtitleText = GameObject.FindGameObjectWithTag("SubtitleTextTag").GetComponent<TextMeshProUGUI>();
        // or if you're using the name to find it:
        // subtitleText = GameObject.Find("SubtitleTextName").GetComponent<TextMeshProUGUI>();

        if (subtitleText == null)
        {
            Debug.LogError("Subtitle Text component not found. Make sure it's present, active, and tagged or named correctly in the scene.");
        }
    }

    public void PlayVoiceLine(VoiceLine voiceLine)
    {
        // Start the coroutine that will show subtitles and play audio
        
        StartCoroutine(PlayVoiceLineCoroutine(voiceLine));
    }

    private IEnumerator PlayVoiceLineCoroutine(VoiceLine voiceLine)
    {
        // Wait for any specified delay before playing the voice line
        yield return new WaitForSeconds(voiceLine.delayBeforePlaying);

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        // Post the Wwise event
        AkSoundEngine.PostEvent(voiceLine.wwiseEventName, player);

        // Assume we have a way to get the duration of the audio event here
        // For now, let's use a placeholder duration value
        float placeholderAudioDuration = 5f; // Placeholder: you will need to determine the actual duration

        if (subtitleText != null)
        {
            // Show the subtitles
            subtitleText.text = voiceLine.subtitle;

            // Wait for the duration of the audio plus any additional time for the subtitles to display
            yield return new WaitForSeconds(placeholderAudioDuration + additionalDisplayTime);

            // Clear the subtitles
            subtitleText.text = "";
        }
    }
}
