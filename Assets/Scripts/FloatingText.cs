using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float destroyTime = 0.9f;
    public Vector3 offset = new Vector3(0, 100, 0);
    public Vector3 randomize;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
        // transform.localScale = new Vector3(1, 0.5f, 1); // Adjust the x value to scale width
        transform.localRotation = Quaternion.Euler(10, 120, 10);
        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-randomize.x, randomize.x), Random.Range(-randomize.y, randomize.y), Random.Range(-randomize.z, randomize.z));
    }
}
