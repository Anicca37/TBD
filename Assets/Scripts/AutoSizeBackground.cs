using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class AutoSizeBackground : MonoBehaviour
{
    public TMP_Text subtitleText; // Assign in inspector
    public Vector2 padding; // How much padding to add around the text

    private RectTransform backgroundRectTransform;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        backgroundRectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>(); // Ensure there's a CanvasGroup attached
    }

    void Update()
    {
        // Check if the subtitle text is not null and has content
        if (subtitleText != null && !string.IsNullOrWhiteSpace(subtitleText.text))
        {
            // If there's text, make sure the background is visible
            canvasGroup.alpha = 1; // Or set gameObject.SetActive(true); if not using a CanvasGroup

            // Adjust the size of the background based on the text size plus padding
            backgroundRectTransform.sizeDelta = new Vector2(subtitleText.preferredWidth + padding.x, subtitleText.preferredHeight + padding.y);
        }
        else
        {
            // If there's no text, hide the background
            canvasGroup.alpha = 0; // Or set gameObject.SetActive(false); if not using a CanvasGroup
        }
    }
}
