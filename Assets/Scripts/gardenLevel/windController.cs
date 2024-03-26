using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    // public GameObject[] seeds;
    // public GameObject targetObject; // scale surface
    // public float launchForce = 10f;
    // public float gravitateForce = 5f;
    // private bool hasCompletedWindChimesPuzzle = false;
    public Camera playerCamera;
    public Camera actionCamera;
    public Animator birdAnimator;

    public void LaunchSeedsEastward()
    {
        // bird animation
        birdAnimator.SetTrigger("Deliver");

        // switch camera
        StartCoroutine(SwitchCamera(playerCamera, actionCamera, 0.5f));

        // foreach (GameObject seed in seeds)
        // {
        //     Rigidbody rb = seed.GetComponent<Rigidbody>();
        //     if (rb != null)
        //     {
        //         rb.isKinematic = false; // make sure Rigidbody is not kinematic
        //         rb.AddForce(Vector3.right * launchForce, ForceMode.Impulse); // launc seeds!
        //         StartCoroutine(GravitateToTarget(rb)); // move towards the target
        //     }
        // }
        StartCoroutine(SwitchCamera(actionCamera, playerCamera, 7f));
        Invoke("ReturnBird", 5f);
        CompleteWindChimesPuzzle();

        // Invoke("SwitchCamera(actionCamera, playerCamera)", 5f);
    }

    // IEnumerator GravitateToTarget(Rigidbody seedRb)
    // {
    //     while (Vector3.Distance(seedRb.position, targetObject.transform.position) > 0.5f)
    //     {
    //         Vector3 directionToTarget = (targetObject.transform.position - seedRb.position).normalized;
    //         seedRb.AddForce(directionToTarget * gravitateForce);
    //         yield return new WaitForFixedUpdate(); // wait until next physics update
    //     }

    //     LockSeedMovement(seedRb); // lock the seed's movement once it's close enough to the target
    // }

    // void LockSeedMovement(Rigidbody seedRb)
    // {
    //     seedRb.velocity = Vector3.zero; // stop curr movement
    //     seedRb.isKinematic = true; // prevent further interactions
    //     if (!hasCompletedWindChimesPuzzle)
    //     {
    //         Invoke("CompleteWindChimesPuzzle", 3f); // wait for 5 sec
    //         hasCompletedWindChimesPuzzle = true;
    //     }
    // }

    void CompleteWindChimesPuzzle()
    {
        GardenManager.Instance.CompletePuzzle("WindChimes");
    }

    void ReturnBird()
    {
        birdAnimator.SetTrigger("Return");
    }

    IEnumerator SwitchCamera(Camera cameraToDisable, Camera cameraToEnable, float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraToDisable.gameObject.SetActive(false);
        cameraToEnable.gameObject.SetActive(true);

    }

}
