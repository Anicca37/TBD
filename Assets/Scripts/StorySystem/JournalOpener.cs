using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalOpener : MonoBehaviour, IInteract
{
    private Book bookReference; 

    private void Start()
    {
        bookReference = FindObjectOfType<Book>();
    }
    public void OnMouseDown()
    {
        if (bookReference != null)
        {
            bookReference.ToggleJournal(true);
        }
    }
}
