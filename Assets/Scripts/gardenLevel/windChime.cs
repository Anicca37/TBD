using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windChime : MonoBehaviour
{
    public WindController windController;
    public ParticleSystem birdsParticleSystem;
    public TreeGrowthController treeGrowthController;
    public Transform windDirectionIndicator;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // mouse click
        {
            // check if the mouse is over gameobject
            RaycastHit hitInfo = new RaycastHit();
            if (Camera.main != null)
            {
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit && hitInfo.transform.gameObject == gameObject)
                {
                    OnMouseOver();
                }
            }
        }
    }
    void OnMouseOver()
    {
        // check if the floral puzzle is matched
        // if (!true)
        if (Input.GetMouseButtonDown(0))
        {

            if (GardenManager.Instance.IsFloralMatched())
            {
                switch (gameObject.name)
                {
                    case "Chime1":
                        ChangeWindDirection(Vector3.forward); // North
                        break;
                    case "Chime2":
                        ChangeWindDirection(Vector3.back); // South
                        break;
                    case "Chime3":
                        ChangeWindDirection(Vector3.right); // East
                        break;
                    case "Chime4":
                        ChangeWindDirection(Vector3.left); // West
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Debug.Log("Ah! So many birds!");
                TriggerBirdsAndGrowPlants();
                // GardenManager.Instance.CompletePuzzle("WindChimes");
            }
        }
    }

    void ChangeWindDirection(Vector3 direction)
    {
        if (windDirectionIndicator != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            windDirectionIndicator.rotation = targetRotation;
        }

        if (direction == Vector3.right) // East
        {
            windController.LaunchSeedsEastward();
        }

        Debug.Log($"Changing wind direction to {direction}");
    }

    void TriggerBirdsAndGrowPlants()
    {
        birdsParticleSystem.Play(); // bird flock
        Invoke("GrowTreesAfterBirds", birdsParticleSystem.main.duration); // delay tree growth after birds
        Invoke("ResetGarden", 15f); // reset after 15sec
    }

    void GrowTreesAfterBirds()
    {
        treeGrowthController.GrowTreesAtMainPoints();
    }

    void ResetGarden()
    {
        // reset to initial state
        birdsParticleSystem.Stop();
        birdsParticleSystem.Clear();
        treeGrowthController.ClearAllTrees();
        GardenManager.Instance.ResetPuzzles(); //?
    }
}