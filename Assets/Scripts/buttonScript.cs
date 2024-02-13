using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameManager gameManager;

    void OnMouseDown()
    {
        gameManager.CompletePuzzle("Scales");
    }
}
