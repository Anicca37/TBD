using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowthController : MonoBehaviour
{
    public GameObject treePrefab;
    public List<Vector3> growthPoints;
    public float growthDuration = 3f; // seconds over which each tree will grow
    public float clusterRadius = 2f; // radius 

    // Call this at the desired moment to start growing trees at all specified points
    public void GrowTreesAtMainPoints()
    {
        foreach (Vector3 mainPoint in growthPoints)
        {
            GrowClusterAroundPoint(mainPoint);
        }
    }

    void GrowClusterAroundPoint(Vector3 mainPoint)
    {
        for (int i = 0; i < 5; i++) // 5 trees around each main point
        {
            Vector3 randomPoint = mainPoint + Random.insideUnitSphere * clusterRadius;
            randomPoint.y = mainPoint.y;
            StartCoroutine(GrowTree(randomPoint));
        }
    }

    IEnumerator GrowTree(Vector3 point)
    {
        GameObject tree = Instantiate(treePrefab, point, Quaternion.identity);
        tree.transform.localScale = Vector3.zero;

        float timer = 0;
        while (timer < growthDuration)
        {
            tree.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / growthDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        tree.transform.localScale = Vector3.one;
    }

    public void ClearAllTrees()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>()) // find all trees in the scene
        {
            if (obj.name.StartsWith("Oak_Tree(Clone)"))
            {
                Destroy(obj);
            }
        }
    }


}
