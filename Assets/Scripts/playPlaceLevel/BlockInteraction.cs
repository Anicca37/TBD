using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockInteraction : MonoBehaviour, IInteract
{
    public blockManager blockManager;
    void Start()
    {
        gameObject.tag = "Interactable";
    }

    public void OnMouseDown()
    {
        blockManager.BlockClicked(gameObject);
    }
}
