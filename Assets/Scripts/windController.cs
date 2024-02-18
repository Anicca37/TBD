using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public GameObject[] seeds;
    public GameObject targetObject; // scale surface
    public float launchForce = 10f;
    public float gravitateForce = 5f;
    private bool hasCompletedWindChimesPuzzle = false;


    public void LaunchSeedsEastward()
    {
        foreach (GameObject seed in seeds)
        {
            Rigidbody rb = seed.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // make sure Rigidbody is not kinematic
                rb.AddForce(Vector3.right * launchForce, ForceMode.Impulse); // launc seeds!
                StartCoroutine(GravitateToTarget(rb)); // move towards the target
            }
        }
    }

    IEnumerator GravitateToTarget(Rigidbody seedRb)
    {
        while (Vector3.Distance(seedRb.position, targetObject.transform.position) > 0.5f)
        {
            Vector3 directionToTarget = (targetObject.transform.position - seedRb.position).normalized;
            seedRb.AddForce(directionToTarget * gravitateForce);
            yield return new WaitForFixedUpdate(); // wait until next physics update
        }

        LockSeedMovement(seedRb); // lock the seed's movement once it's close enough to the target
    }

    void LockSeedMovement(Rigidbody seedRb)
    {
        seedRb.velocity = Vector3.zero; // stop curr movement
        seedRb.isKinematic = true; // prevent further interactions
        if (!hasCompletedWindChimesPuzzle)
        {
            GardenManager.Instance.CompletePuzzle("WindChimes");
            hasCompletedWindChimesPuzzle = true;
        }
    }
}
