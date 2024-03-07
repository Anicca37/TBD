// using UnityEngine;

// [RequireComponent(typeof(Renderer))] // Ensure there is a Renderer component
// public class InteractableOutline : MonoBehaviour
// {
//     private Renderer renderer; // Reference to the Renderer component
//     private Material originalMaterial; // To keep track of the original material
//     public Material outlineMaterial; // The outline material to swap to
//     public bool isLookingAt = false; // Controlled by another script, e.g., player gaze detection

//     void Start()
//     {
//         renderer = GetComponent<Renderer>();
//         originalMaterial = renderer.material; // Save the original material
//     }

//     void Update()
//     {
//         // Check if the player is looking at the object and swap materials accordingly
//         if (isLookingAt)
//         {
//             if (renderer.material != outlineMaterial)
//             {
//                 renderer.material = outlineMaterial; // Apply the outline material
//             }
//         }
//         else
//         {
//             if (renderer.material != originalMaterial)
//             {
//                 renderer.material = originalMaterial; // Revert to the original material
//             }
//         }
//     }

//     // Optionally, a method to manually set 'isLookingAt', could be called by a raycast from the player's camera
//     public void SetIsLookingAt(bool lookingAt)
//     {
//         isLookingAt = lookingAt;
//     }
// }

//^ for just one object, use code above
//for all children objects as well, use code underneath

using UnityEngine;

public class InteractableOutline : MonoBehaviour
{
    // Struct to hold the renderer and its original materials
    private struct RendererMaterials
    {
        public Renderer Renderer;
        public Material[] OriginalMaterials;
    }

    private RendererMaterials[] childRenderersMaterials; // Array to hold all child renderers and their materials
    public Material outlineMaterial; // The outline material to swap to
    public bool isLookingAt = false; // Controlled by another script

    void Start()
    {
        // Get all renderers in this object and its children
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        childRenderersMaterials = new RendererMaterials[childRenderers.Length];

        // Save the original materials of all children
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderersMaterials[i].Renderer = childRenderers[i];
            childRenderersMaterials[i].OriginalMaterials = childRenderers[i].materials; // Notice 'materials' not 'material'
        }
    }

    void Update()
    {
        // Check if the player is looking at the object and swap materials accordingly
        if (isLookingAt)
        {
            foreach (var rendererMaterials in childRenderersMaterials)
            {
                Material[] outlineMaterials = new Material[rendererMaterials.OriginalMaterials.Length];
                for (int i = 0; i < outlineMaterials.Length; i++)
                {
                    outlineMaterials[i] = outlineMaterial; // Use the outline material for all sub-materials
                }

                rendererMaterials.Renderer.materials = outlineMaterials;
            }
        }
        else
        {
            // Revert to the original materials
            foreach (var rendererMaterials in childRenderersMaterials)
            {
                rendererMaterials.Renderer.materials = rendererMaterials.OriginalMaterials;
            }
        }
    }

    public void SetIsLookingAt(bool lookingAt)
    {
        isLookingAt = lookingAt;
    }
}

