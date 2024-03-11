using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowthController : MonoBehaviour
{
    public GameObject treePrefab;
    public List<Vector3> growthPoints;
    public float growthDuration = 3f; // Seconds over which each tree will grow and shrink
    public float clusterRadius = 2f; // Radius around each growth point to spawn trees
    public LayerMask groundLayer; // Assign in the inspector to match your terrain's layer
    private List<GameObject> grownTrees = new List<GameObject>(); // Track all grown trees

    void Start()
    {
        // Example call to grow trees at start, can be triggered by any event
        // GrowTreesAtMainPoints();
    }

    public void GrowTreesAtMainPoints()
    {
        //play sound
        AkSoundEngine.PostEvent("Play_WoodGrowingCrazy", this.gameObject);

        foreach (Vector3 mainPoint in growthPoints)
        {
            GrowClusterAroundPoint(mainPoint);
        }
    }

    void GrowClusterAroundPoint(Vector3 mainPoint)
    {
        for (int i = 0; i < 15; i++) // 15 trees around each main point
        {
            Vector3 randomPoint = mainPoint + Random.insideUnitSphere * clusterRadius;
            randomPoint.y = mainPoint.y + 10; // Start from above to ensure raycast hits ground
            Vector3 groundPoint = FindGroundPoint(randomPoint);
            if (groundPoint != Vector3.zero) // Valid ground point found
            {
                StartCoroutine(GrowTree(groundPoint));
            }
        }
    }

    Vector3 FindGroundPoint(Vector3 startPoint)
    {
        RaycastHit hit;
        if (Physics.Raycast(startPoint, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }
        return Vector3.zero; // Return zero vector if ground not found
    }

    IEnumerator GrowTree(Vector3 point)
    {
        GameObject tree = Instantiate(treePrefab, point, Quaternion.identity);
        grownTrees.Add(tree); // Add the new tree to the list
        tree.transform.localScale = Vector3.zero;

        float timer = 0;
        while (timer < growthDuration)
        {
            tree.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 3, timer / growthDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        tree.transform.localScale = Vector3.one * 3; // Final scale set to 3 times original, adjust as needed
    }

    public void RemoveTreesAfterGrowth()
    {
        StartCoroutine(ShrinkAndRemoveTrees());
    }

    IEnumerator ShrinkAndRemoveTrees()
    {
        foreach (GameObject tree in grownTrees)
        {
            StartCoroutine(ShrinkTree(tree));
        }

        // Wait for all trees to finish shrinking
        yield return new WaitForSeconds(growthDuration + 0.5f); // Extra half second for safety

        // Clear all trees from the scene
        grownTrees.Clear(); // Ensure the list is cleared after all trees are destroyed
    }

    IEnumerator ShrinkTree(GameObject tree)
    {
        float timer = 0;
        Vector3 originalScale = tree.transform.localScale;
        while (timer < growthDuration)
        {
            if (tree == null) // Check if the tree has already been destroyed
                yield break; // If so, exit the coroutine

            tree.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, timer / growthDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        if (tree != null) // Check again before destroying
            Destroy(tree); // Remove tree after shrinking
    }


    // This function can be improved for efficiency by keeping track of trees in a list, as done in GrowTree
    public void ClearAllTrees()
    {
        foreach (GameObject tree in grownTrees)
        {
            Destroy(tree);
        }
        grownTrees.Clear(); // Clear the list after removing all trees
    }
}
