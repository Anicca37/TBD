using System.Collections;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleText; 
    public float displayTime = 4.0f;
    private AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(ShowSubtitles());
    }

    IEnumerator ShowSubtitles()
    {

        yield return StartCoroutine(TypeSubtitle("Ecape trauma: Eaten by a venus flytrap"));
        subtitleText.text = ""; 
    }

    IEnumerator TypeSubtitle(string message)
    {
        subtitleText.text = ""; 
        foreach (char letter in message.ToCharArray())
        {
            subtitleText.text += letter;
            yield return new WaitForSeconds(0.05f); 
        }
        yield return new WaitForSeconds(displayTime); 
    }
}
