using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockInteraction : MonoBehaviour, IInteract
{
    public blockManager blockManager;

    public void OnMouseDown()
    {
        blockManager.BlockClicked(gameObject);
    }
}
