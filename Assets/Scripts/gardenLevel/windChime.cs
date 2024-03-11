using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windChime : MonoBehaviour
{
    public WindController windController;
    public ParticleSystem birdsParticleSystem;
    public TreeGrowthController treeGrowthController;
    public Transform windDirectionIndicator;
    // private int[] targetSequence = { 4, 3, 2, 1 };
    // private int currentSequenceIndex = 0;
    public int chimeID;

    void OnMouseDown()
    {
        // check if the floral puzzle is matched
        // if (!true)
        if (GardenManager.Instance.IsFloralMatched())
        {
            // Debug.Log($"curr index: {currentSequenceIndex}");
            // Debug.Log($"{chimeID} = {targetSequence[currentSequenceIndex]} is {chimeID == targetSequence[currentSequenceIndex]}");
            ChangeWindDirection(chimeID);
            FindObjectOfType<SequenceChecker>().ChimeClicked(chimeID);

            // if (chimeID == targetSequence[currentSequenceIndex])
            // {
            //     // Move to the next chime in the sequence
            //     currentSequenceIndex++;
            //     Debug.Log($"next target: {currentSequenceIndex}, {targetSequence[currentSequenceIndex]}");
            //     // Check if the entire sequence has been correctly entered
            //     if (currentSequenceIndex >= targetSequence.Length)
            //     {
            //         // Sequence complete, launch seeds and reset sequence index
            //         Debug.Log("slay");
            //         windController.LaunchSeedsEastward();
            //         currentSequenceIndex = 0; // Reset for next sequence attempt
            //     }
            // }
            // else
            // {
            //     // Incorrect chime, reset sequence index
            //     Debug.Log("reset");
            //     currentSequenceIndex = 0;
            // }
        }
        else
        {
            Debug.Log("Ah! So many birds!");
            TriggerBirdsAndGrowPlants();
            // GardenManager.Instance.CompletePuzzle("WindChimes");
        }
    }

    void ChangeWindDirection(int chimeID)
    {
        Vector3 direction = Vector3.zero;
        switch (chimeID)
        {
            case 1:
                direction = Vector3.forward; // North
                break;
            case 2:
                direction = Vector3.back; // South
                break;
            case 3:
                direction = Vector3.right; // East
                break;
            case 4:
                direction = Vector3.left; // West
                break;
        }

        if (windDirectionIndicator != null && direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            windDirectionIndicator.rotation = targetRotation;
        }
        Debug.Log($"curr chime: {chimeID}");
        Debug.Log($"wind direction: {direction}");

        // play sound
        AkSoundEngine.PostEvent("Play_ChimeD", this.gameObject);
    }

    void TriggerBirdsAndGrowPlants()
    {
        birdsParticleSystem.Play(); // bird flock
        Invoke("GrowTreesAfterBirds", birdsParticleSystem.main.duration); // delay tree growth after birds
        Invoke("ResetGarden", 8f); // reset after 8sec


        //play bird sounds multiple times
        StartCoroutine(PlayBirdSoundMultipleTimes());
    }

    IEnumerator PlayBirdSoundMultipleTimes()
    {
        // play sound
        while (true)
        {
            AkSoundEngine.PostEvent("Play_BirdsCrazy", this.gameObject);
            yield return new WaitForSeconds(0.1f);
        }
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