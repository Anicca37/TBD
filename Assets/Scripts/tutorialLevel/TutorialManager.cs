using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public GameObject EscapeController;
    public DoorMovement doorMovement;

    private float drawForce = 2f;
    private float distanceToPlayer = 15f;
    private GameObject player;
    private CharacterController playerController;
    private GameObject[] children;

    public Transform playerTransform;
    public Transform pushyBird;
    public float pushCooldown = 1f;
    private float lastPushTime = -Mathf.Infinity;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
            player = GameObject.Find("Player");
            playerController = player.GetComponent<CharacterController>();

            // get current object's children
            children = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i).gameObject;
            }
            SetSpiral(false);

            //AkSoundEngine.PostEvent("Play_Level0Music", this.gameObject);                       
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (doorMovement.IsDoorOpen())
        {
            SetSpiral(true);
        }

        if (IsPlayerNearby() && doorMovement.IsDoorOpen())
        {
            DrawInward();
        }
    }

    public void ResetPuzzles()
    {
        //stop music
        AkSoundEngine.PostEvent("Stop_Level0Music", this.gameObject);

        GameObject TheClock = GameObject.Find("Clock");
        AkSoundEngine.PostEvent("Stop_Clock_Tick", TheClock.gameObject);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the tutorial area.");
            EscapeTutorial();
        }
    }

    private void EscapeTutorial()
    {
        Debug.Log("Escaping the office.");
        EscapeController.GetComponent<EscapeMenuController>().OnEscapeActivated();
        AkSoundEngine.PostEvent("Play_Win", this.gameObject);
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

    void SetSpiral(bool active)
    {
        foreach (GameObject child in children)
        {
            child.SetActive(active);
        }
    }

    public void BirdPush()
    {
        if (Time.time >= lastPushTime + pushCooldown)
        {
            Debug.Log("Big push.");
            StartCoroutine(PushPlayer());
            lastPushTime = Time.time;
        }
        else
        {
            Debug.Log("Push cooldown.");
        }
    }

    IEnumerator PushPlayer()
    {
        float duration = 1f;
        float startTime = Time.time; // start time
        float upwardSpeed = 10.0f; // speed player is pushed upward
        float backwardSpeed = -20.0f; // speed player is pushed backward

        //play sound

        while (Time.time < startTime + duration)
        {
            CharacterController controller = playerTransform.GetComponent<CharacterController>();
            Vector3 moveDirection = playerTransform.forward * backwardSpeed + Vector3.up * upwardSpeed;
            controller.Move(moveDirection * Time.deltaTime);

            yield return null; // Wait until the next frame
        }
    }
}
