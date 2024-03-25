using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XylophoneClick : MonoBehaviour, IInteract
{
    public int xyloID;

    public XSequenceChecker XsequenceChecker;

    public void OnMouseDown()
    {
        FindObjectOfType<XSequenceChecker>().XyloClicked(xyloID);
        PlayXyloSound(xyloID);
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
}