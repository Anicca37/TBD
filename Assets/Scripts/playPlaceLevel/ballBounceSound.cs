using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballBounceSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        AkSoundEngine.PostEvent("Play_BallImpact", this.gameObject);
        Debug.Log("Ball impacted");
    }
}
