using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SketchInteraction : MonoBehaviour, IInteract
{
    public GameObject inactivePageSprite;
    public GameObject activePageSprite;  

    public void OnMouseDown()
    {
        if (Book.Instance != null)
        {
            Book.Instance.UpdatePageSprites(inactivePageSprite, activePageSprite);
        }

        gameObject.SetActive(false);

        Book.Instance.ToggleJournal(false);
    }
}
