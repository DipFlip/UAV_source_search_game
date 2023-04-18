using UnityEngine;

public class GeigerCounter : MonoBehaviour
{
    public AudioClip geigerSound;
    public float minDistance = 1f;
    public float maxDistance = 10f;
    public float minInterval = 0.2f;
    public float maxInterval = 2f;
    public float randomFactor = 0.5f;

    public float distanceToSource;

    private AudioSource audioSource;
    private float timeSinceLastPlay;
    private float nextPlayInterval;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("GeigerCounter requires an AudioSource component.");
        }
    }

    private void Start()
    {
        UpdateNextPlayInterval();
    }

    private void Update()
    {
        if (audioSource != null)
        {
            timeSinceLastPlay += Time.deltaTime;

            if (timeSinceLastPlay >= nextPlayInterval)
            {
                audioSource.PlayOneShot(geigerSound);
                timeSinceLastPlay = 0;
                UpdateNextPlayInterval();
            }
        }
    }

    private void UpdateNextPlayInterval()
    {
        // Calculate the play interval based on the distanceToSource value
        float t = Mathf.InverseLerp(minDistance, maxDistance, distanceToSource);
        float baseInterval = Mathf.Lerp(minInterval, maxInterval, t);

        // Apply randomness to the interval
        float randomInterval = Random.Range(-randomFactor, randomFactor) * baseInterval;
        nextPlayInterval = baseInterval + randomInterval;
    }
}
