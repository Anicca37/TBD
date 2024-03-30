using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenusFlytrapController : MonoBehaviour
{
    private Animator venusFlytrapAnimator;
    private bool IsGrowed = false;

    // Start is called before the first frame update
    void Start()
    {
        venusFlytrapAnimator = GetComponent<Animator>();
    }

    public void VenusFlytrapGrow()
    {
        venusFlytrapAnimator.SetTrigger("grow");
        IsGrowed = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsGrowed)
            {
                venusFlytrapAnimator.SetTrigger("eat");
                GardenManager.Instance.CompletePuzzle("Escape");
            }
        }
    }
}
