using UnityEngine;

public class buttonScript : MonoBehaviour
{
    public GardenManager GardenManager;

    // This function will be called from the player controller when the button is interacted with.
    public void Interact()
    {
        // Complete the puzzle and play sound only if this button is interacted with.
        GardenManager.CompletePuzzle("Scales");
        AkSoundEngine.PostEvent("Play_Birds", gameObject);
    }
}
