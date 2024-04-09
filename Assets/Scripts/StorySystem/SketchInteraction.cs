using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SketchInteraction : MonoBehaviour, IInteract
{
    public List<GameObject> inactivePageSprites;
    public List<GameObject> activePageSprites;
    public int pageNumberToOpen;

    private Book bookReference; 

    private void Start()
    {
        bookReference = FindObjectOfType<Book>();
    }

    public void OnMouseDown()
    {
        if (bookReference != null)
        {
            bookReference.UpdatePageSprites(inactivePageSprites, activePageSprites);
            bookReference.OpenPage(pageNumberToOpen + 1);
        }
        gameObject.SetActive(false);
    }
}
