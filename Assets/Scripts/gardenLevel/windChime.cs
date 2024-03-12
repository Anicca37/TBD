using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windChime : MonoBehaviour
{
    public WindController windController;
    public ParticleSystem birdsParticleSystem;
    public TreeGrowthController treeGrowthController;
    public Transform windDirectionIndicator;

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
                // TODO: add sound
                AkSoundEngine.PostEvent("Play_ChimeG", this.gameObject);
                direction = Vector3.forward; // North
                break;
            case 2:
                // TODO: add sound
                AkSoundEngine.PostEvent("Play_ChimeE", this.gameObject);
                direction = Vector3.back; // South
                break;
            case 3:
                // TODO: add sound
                AkSoundEngine.PostEvent("Play_ChimeD", this.gameObject);
                direction = Vector3.right; // East
                break;
            case 4:
                // TODO: add sound
                AkSoundEngine.PostEvent("Play_ChimeC", this.gameObject);
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
        // AkSoundEngine.PostEvent("Play_ChimeD", this.gameObject);
    }

    void TriggerBirdsAndGrowPlants()
    {
        birdsParticleSystem.Play(); // bird flock
        StartCoroutine(PlayBirdSoundMultipleTimes());
        Invoke("GrowTreesAfterBirds", birdsParticleSystem.main.duration); // delay tree growth after birds
        Invoke("RemoveTreesAfterGrowth", 12f); // reset after 8sec
        // Invoke("ResetGarden", 8f); // reset after 8sec

    }

    IEnumerator PlayBirdSoundMultipleTimes()
    {
        float duration = 5.0f; // Duration in seconds after which the sound should stop
        float startTime = Time.time; // Record the start time

        // play sound
        while (Time.time - startTime < duration)
        {
            AkSoundEngine.PostEvent("Play_BirdsCrazy", this.gameObject);
            yield return new WaitForSeconds(0.1f);
        }
    }


    void GrowTreesAfterBirds()
    {
        treeGrowthController.GrowTreesAtMainPoints();
    }

    void RemoveTreesAfterGrowth()
    {
        treeGrowthController.RemoveTreesAfterGrowth();
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