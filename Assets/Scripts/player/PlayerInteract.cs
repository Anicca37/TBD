using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public string InteractableTag = "Interactable";
    public float InteractRange = 30f;
    private GameObject InteractedObject;

    void Update()
    {
        InteractObject();
    }

    public bool CanInteract()
    {
        if (Camera.main == null)
        {
            return false;
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, InteractRange))
        {
            if (hit.collider.CompareTag(InteractableTag))
            {
                InteractedObject = hit.collider.gameObject;
                return true;
            }
        }
        return false;
    }

    // Interact Object
    void InteractObject()
    {
        if (CanInteract() && Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Interacted with " + InteractedObject.name);
            IInteract[] monoBehaviours = InteractedObject.GetComponentsInChildren<IInteract>();
            foreach (IInteract monoBehaviour in monoBehaviours)
            {
                monoBehaviour.OnMouseDown();
            }
        }
    }
}
