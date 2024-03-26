using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalOpener : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (Book.Instance != null)
        {
            Book.Instance.ToggleJournal(false);
        }
    }
}
