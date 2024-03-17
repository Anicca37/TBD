using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockInteraction : MonoBehaviour
{
    public blockManager blockManager;

    private void OnMouseDown()
    {
        blockManager.BlockClicked(gameObject);
    }
}
