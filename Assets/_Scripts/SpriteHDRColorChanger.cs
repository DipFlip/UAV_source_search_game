using UnityEngine;

public class SpriteHDRColorChanger : MonoBehaviour
{
    public Color hdrColor;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteHDRColorChanger requires a SpriteRenderer component.");
        }
    }

    private void Update()
    {
        SetHDRColor();
    }

    public void SetHDRColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hdrColor;
        }
    }
}
