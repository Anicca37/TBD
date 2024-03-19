using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayplaceEscapeDetector : MonoBehaviour
{
    private float drawForce = 2f;
    private float distanceToPlayer = 10f;
    private GameObject player;
    private CharacterController playerController;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerNearby() && PlayPlaceManager.Instance.GetIsXylophoneSequenceCorrect())
        {
            DrawInward();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the tunnel");
            PlayPlaceManager.Instance.CompletePuzzle("Escape");
        }
    }

    bool IsPlayerNearby()
    {
        // check if player is nearby
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance < distanceToPlayer;
    }

    void DrawInward()
    {
        Vector3 direction = transform.position - player.transform.position;
        playerController.Move(direction * drawForce * Time.deltaTime);
    }
}
