using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.ConfirmInput)
        {
            SceneManager.LoadScene("UI");
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(25f);
        // Load the next scene
        SceneManager.LoadScene("UI");
    }
}
