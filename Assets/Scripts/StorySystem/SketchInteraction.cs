using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SketchInteraction : MonoBehaviour
{
    public GameObject inactivePageSprite;
    public GameObject activePageSprite;

    private Book bookReference; 

    private void Start()
    {
        bookReference = FindObjectOfType<Book>();
    }

    private void OnMouseDown()
    {
        if (bookReference != null)
        {
            bookReference.UpdatePageSprites(inactivePageSprite, activePageSprite);
            bookReference.ToggleJournal(true);
        }
        gameObject.SetActive(false);
    }
}
