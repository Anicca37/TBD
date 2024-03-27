using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SketchInteraction : MonoBehaviour, IInteract
{
    public GameObject inactivePageSprite;
    public GameObject activePageSprite;

    private Book bookReference; 

    private void Start()
    {
        bookReference = FindObjectOfType<Book>();
    }

    public void OnMouseDown()
    {
        if (bookReference != null)
        {
            bookReference.UpdatePageSprites(inactivePageSprite, activePageSprite);
            if (SceneManager.GetActiveScene().name == "PlayPlace")
            {
                bookReference.OpenPage(2);  
            }
            else if (SceneManager.GetActiveScene().name == "Garden_3 - Terrain")
            {
                bookReference.OpenPage(3);
            }
            else
            {
                bookReference.ToggleJournal(true);
            }
        }
        gameObject.SetActive(false);
    }
}
