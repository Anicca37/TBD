using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windChime : MonoBehaviour
{
    public ParticleSystem windParticleSystem;
    public WindController windController;
    public ParticleSystem birdsParticleSystem;
    public TreeGrowthController treeGrowthController; // Assign in the inspector


    void OnMouseDown()
    {
        // Debug.Log("GM.Instance: " + GardenManager.Instance);
        // check if the floral puzzle is matched
        // if (!true)
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

    void ChangeWindDirection(Vector3 direction)
    {
        var shape = windParticleSystem.shape;
        Quaternion rotation = Quaternion.LookRotation(direction);
        shape.rotation = rotation.eulerAngles;

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