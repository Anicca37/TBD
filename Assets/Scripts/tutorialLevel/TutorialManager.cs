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
}
