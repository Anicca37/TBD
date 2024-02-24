using UnityEngine;

public class materialTiling : MonoBehaviour
{
    public float tileX = 2f;
    public float tileY = 2f;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            Material mat = rend.material;
            if (mat != null)
            {
                mat.mainTextureScale = new Vector2(tileX, tileY);
            }
        }
    }
}
