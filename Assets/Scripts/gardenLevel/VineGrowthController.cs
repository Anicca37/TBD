using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrowthController : MonoBehaviour
{
    public List<GameObject> vines;
    public Vector3 maxScale = new Vector3(10f, 10f, 10f);
    private Dictionary<GameObject, Vector3> originalVineScales = new Dictionary<GameObject, Vector3>();
    private bool IsGrowed = false;

    void Start()
    {
        // Initialize the vines with a scale of 0 (invisible)
        foreach (var vine in vines)
        {
            // vine.transform.localScale = Vector3.zero;
            // originalVineScales[vine] = maxScale;
            originalVineScales[vine] = new Vector3(vine.transform.position.x, 13f, vine.transform.position.z);
            vine.transform.position = new Vector3(vine.transform.position.x, 13f, vine.transform.position.z);
        }
    }

    // Call this method to update the vines' growth based on the clock's rotation
    public void UpdateVineGrowth(float rotationAmount)
    {
        if (rotationAmount > 0.5f || IsGrowed)
        {
            return;
        }
        float growthFactor = 1f + Mathf.Clamp((Mathf.Abs(rotationAmount) % 360) / 360f, 0f, 1f);
        if (growthFactor > 1.6f)
        {
            IsGrowed = true;
            return;
        }
        foreach (var vine in vines)
        {
            // change vine position.y based on growth factor
            vine.transform.position = new Vector3(vine.transform.position.x, growthFactor * originalVineScales[vine].y, vine.transform.position.z);
        }
    }
}
