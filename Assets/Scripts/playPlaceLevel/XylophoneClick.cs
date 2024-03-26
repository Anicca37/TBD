using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XylophoneClick : MonoBehaviour, IInteract
{
    public int xyloID;
    [SerializeField] private Animator xyloAnimator;
    public GameObject floatingText;

    public void OnMouseDown()
    {
        xyloAnimator.SetTrigger($"Hit {xyloID}"); // animate hit
        PlayXyloSound(xyloID);
        StartCoroutine(ResetAnimation(xyloID, 1f));
        if (floatingText)
        {
            ShowDing();
        }
        Invoke("CheckSequence", 0.5f);
    }

    void PlayXyloSound(int xyloID)
    {
        Vector3 direction = Vector3.zero;
        switch (xyloID)
        {
            case 1:
                AkSoundEngine.PostEvent("Play_XyloC", this.gameObject);
                break;
            case 2:
                AkSoundEngine.PostEvent("Play_XyloD", this.gameObject);
                break;
            case 3:
                AkSoundEngine.PostEvent("Play_XyloE", this.gameObject);
                break;
            case 4:
                AkSoundEngine.PostEvent("Play_XyloG", this.gameObject);
                break;
        }
    }

    void ShowDing()
    {
        Instantiate(floatingText, transform.position, Quaternion.identity, transform);
    }

    void CheckSequence()
    {
        if (!PlayPlaceManager.Instance.IsXylophoneSequenceCorrect)
        { FindObjectOfType<XSequenceChecker>().XyloClicked(xyloID); }
    }

    IEnumerator ResetAnimation(int id, float delay)
    {
        yield return new WaitForSeconds(delay);
        xyloAnimator.SetTrigger($"Return {id}");
    }
}