using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowthController : MonoBehaviour
{
    public GameObject treePrefab;
    public List<Vector3> growthPoints;
    public float growthDuration = 2f;
    public float clusterRadius = 2f;
    public LayerMask groundLayer;
    private List<GameObject> grownTrees = new List<GameObject>();

    private bool isWoodSoundPlayed = false;
    private bool isGrowing = false; // Flag to indicate if trees are currently growing
    private bool isRemovingTrees = false; // Flag to indicate if trees are currently being removed

    private void Start()
    {
        // Example call to grow trees at start, can be triggered by any event
        // GrowTreesAtMainPoints();
    }

    public void GrowTreesAtMainPoints()
    {
        // Prevent growing new trees if we're already growing or removing trees
        if (isGrowing || isRemovingTrees)
        {
            return;
        }

        isGrowing = true;
        
        if (!isWoodSoundPlayed)
        {
            isWoodSoundPlayed = true;
            AkSoundEngine.PostEvent("Play_WoodGrowingCrazy", gameObject);
            Invoke("ResetWoodSound", 8f);
        }

        foreach (Vector3 mainPoint in growthPoints)
        {
            GrowClusterAroundPoint(mainPoint);
        }
    }

    private void ResetWoodSound()
    {
        isWoodSoundPlayed = false;
    }

    private void GrowClusterAroundPoint(Vector3 mainPoint)
    {
        for (int i = 0; i < 15; i++)
        {
            Vector3 randomPoint = mainPoint + Random.insideUnitSphere * clusterRadius;
            randomPoint.y = mainPoint.y + 10;
            Vector3 groundPoint = FindGroundPoint(randomPoint);
            if (groundPoint != Vector3.zero)
            {
                StartCoroutine(GrowTree(groundPoint));
            }
        }
    }

    private Vector3 FindGroundPoint(Vector3 startPoint)
    {
        RaycastHit hit;
        if (Physics.Raycast(startPoint, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private IEnumerator GrowTree(Vector3 point)
    {
        GameObject tree = Instantiate(treePrefab, point, Quaternion.identity);
        grownTrees.Add(tree);
        tree.transform.localScale = Vector3.zero;

        float timer = 0;
        while (timer < growthDuration)
        {
            tree.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 3, timer / growthDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        tree.transform.localScale = Vector3.one * 3; // Final scale set
        isGrowing = false; // Allow new trees to grow now that this tree has finished
    }

    public void RemoveTreesAfterGrowth()
    {
        if (isRemovingTrees)
        {
            return; // Already removing trees, so do nothing
        }

        isRemovingTrees = true; // Flag that we are starting to remove trees
        StartCoroutine(ShrinkAndRemoveTrees());
    }

    private IEnumerator ShrinkAndRemoveTrees()
    {
        foreach (GameObject tree in grownTrees)
        {
            StartCoroutine(ShrinkTree(tree));
        }

        // Wait for all trees to finish shrinking
        yield return new WaitForSeconds(growthDuration + 0.5f);

        grownTrees.Clear(); // Clear the list after all trees are destroyed
        isRemovingTrees = false; // We can now grow new trees
    }

    private IEnumerator ShrinkTree(GameObject tree)
    {
        float timer = 0;
        Vector3 originalScale = tree.transform.localScale;

        while (timer < growthDuration)
        {
            if (tree == null) // Tree might have been destroyed externally
                yield break;

            tree.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, timer / growthDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        if (tree != null)
            Destroy(tree);
    }

    public void ClearAllTrees()
    {
        foreach (GameObject tree in grownTrees)
        {
            Destroy(tree);
        }
        grownTrees.Clear(); // Clear the list after removing all trees
    }
}
