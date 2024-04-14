using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenusFlytrapController : MonoBehaviour
{
    private Animator venusFlytrapAnimator;
    private bool IsGrowed = false;
    private bool IsPlayerEaten = false;
    private GameObject theFlytrap;

    // Start is called before the first frame update
    void Start()
    {
        venusFlytrapAnimator = GetComponent<Animator>();   
        theFlytrap = GameObject.Find("body.002");
    }

    public void VenusFlytrapGrow()
    {
        venusFlytrapAnimator.SetTrigger("grow");
        Invoke("playFlytrapSound", 0.2f);        
        IsGrowed = true;

        Invoke("startBreathSounds", 5f);
    }

    private void startBreathSounds()
    {
        StartCoroutine(PlayFlytrapBreathSound());
    }

    private IEnumerator PlayFlytrapBreathSound()
    {
        //GameObject theFlytrap = GameObject.Find("body.002");
        while (IsGrowed && !IsPlayerEaten)
        {
            
            AkSoundEngine.PostEvent("Play_FlytrapBreath", theFlytrap.gameObject);

            yield return new WaitForSeconds(15f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsGrowed)
            {
                venusFlytrapAnimator.SetTrigger("eat");
                IsPlayerEaten = true;
                GardenManager.Instance.CompletePuzzle("Escape");
            }
        }
    }

    void playFlytrapSound()
    {
        //GameObject theFlytrap = GameObject.Find("body.002");
        AkSoundEngine.PostEvent("Play_Vine_Growing", theFlytrap.gameObject);
    }
}
