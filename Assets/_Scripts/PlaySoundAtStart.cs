using System.Collections;
using UnityEngine;

public class PlaySoundAtStart : MonoBehaviour
{
    public AudioClip audioClip;
    public float delayInSeconds = 0.2f;

    private AudioSource audioSource;

    private void Start()
    {
        SoundManager.Instance.PlaySoundDelayed(audioClip, delayInSeconds);
    }
}
