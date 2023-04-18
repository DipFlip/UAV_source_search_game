using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Declare the audio sources and clips
    public AudioSource audioSourceEffects;
    public AudioSource audioSourceMusic;
    public AudioClip scoreSound;
    public AudioClip wrongSound;
    public AudioClip backgroundMusic;

    // Add variables for randomness and delay
    public float pitchRandomness = 0.1f;

    // Singleton pattern
    public static SoundManager Instance;

    private void Awake()
    {
        // Implement the Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Start playing the background music
        PlayBackgroundMusic();
    }

    public void PlayScoreSound()
    {
        PlayRandomizedSound(scoreSound);
    }

    public void PlayWrongSound()
    {
        PlayRandomizedSound(wrongSound);
    }

    public void PlaySoundDelayed(AudioClip clip, float delay)
    {
        StartCoroutine(PlaySoundDelayedEnum(clip, delay));
    }

    private void PlayBackgroundMusic()
    {
        audioSourceMusic.clip = backgroundMusic;
        audioSourceMusic.loop = true;
        audioSourceMusic.Play();
    }

    private void PlayRandomizedSound(AudioClip clip)
    {
        float randomPitch = Random.Range(1 - pitchRandomness, 1 + pitchRandomness);
        audioSourceEffects.pitch = randomPitch;
        audioSourceEffects.PlayOneShot(clip);
    }

    private IEnumerator PlaySoundDelayedEnum(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayRandomizedSound(clip);
    }
}