using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdWindow : MonoBehaviour
{
    [SerializeField] private Animator bird;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Chirp");
            bird.SetTrigger("Warn");
            Invoke("ResetBird", 4.5f);
        }
    }
    private void ResetBird()
    {
        bird.SetTrigger("Warn Return");
    }
}
