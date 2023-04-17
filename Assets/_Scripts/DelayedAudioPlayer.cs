using System.Collections;
using UnityEngine;

public class DelayedAudioPlayer : MonoBehaviour
{
    public AudioClip audioClip;
    public float delayInSeconds = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        StartCoroutine(PlayAudioWithDelay());
    }

    private IEnumerator PlayAudioWithDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        audioSource.Play();
    }
}
