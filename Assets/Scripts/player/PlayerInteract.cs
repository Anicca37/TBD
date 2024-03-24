using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public string InteractableTag = "Interactable";
    public string PickUpableTag = "Pickupable";
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
            if (hit.collider.CompareTag(InteractableTag) || hit.collider.CompareTag(PickUpableTag))
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
        if (CanInteract() && Input.GetButtonDown("Fire3"))
        {
            IInteract[] monoBehaviours = InteractedObject.GetComponentsInChildren<IInteract>();
            foreach (IInteract monoBehaviour in monoBehaviours)
            {
                monoBehaviour.OnMouseDown();
            }
        }
    }
}
