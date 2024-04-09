using UnityEngine;

[CreateAssetMenu(fileName = "VoiceLine", menuName = "Audio/VoiceLine", order = 1)]
public class VoiceLine : ScriptableObject
{
    public string wwiseEventName; // The name of the Wwise event to post
    [TextArea] public string subtitle; // The subtitle text to display
    public float delayBeforePlaying = 0f; // Delay in seconds before the voice line plays
}
