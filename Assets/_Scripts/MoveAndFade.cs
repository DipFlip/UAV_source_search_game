using UnityEngine;
using DG.Tweening;

public class MoveAndFade : MonoBehaviour
{
    public float moveDistance = 5f;
    public float duration = 1f;
    public Ease moveEase = Ease.OutSine;
    public Ease fadeEase = Ease.Linear;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("MoveAndFade requires a SpriteRenderer component.");
        }
    }
    private void Start()
    {
        MoveUpAndFadeOut();
    }

    private void MoveUpAndFadeOut()
    {
        if (spriteRenderer != null)
        {
            // Move up
            transform.DOMoveY(transform.position.y + moveDistance, duration).SetEase(moveEase);

            // Fade out
            spriteRenderer.DOFade(0, duration).SetEase(fadeEase);
        }
    }
}
