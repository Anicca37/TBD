using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenusFlytrapController : MonoBehaviour
{
    private bool PlayerEaten = false; //For OneTimeSound
    private Animator venusFlytrapAnimator;


    // Start is called before the first frame update
    void Start()
    {
        venusFlytrapAnimator = GetComponent<Animator>();
    }

    public void VenusFlytrapGrow()
    {
        venusFlytrapAnimator.SetTrigger("grow");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            venusFlytrapAnimator.SetTrigger("eat");
            Debug.Log("Player entered the Venus Flytrap");
            GardenManager.Instance.CompletePuzzle("Escape");
        }
    }
}
