using UnityEngine;

public class HoverObject : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 startPosition;
    private float elapsedTime;

    private void Start()
    {
        startPosition = transform.position;
        elapsedTime = 0f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float yPosition = startPosition.y + amplitude * Mathf.Sin(2 * Mathf.PI * frequency * elapsedTime);
        transform.position = new Vector3(startPosition.x, yPosition, startPosition.z);
        transform.position = new Vector3(startPosition.x, yPosition, startPosition.z);
    }
}
