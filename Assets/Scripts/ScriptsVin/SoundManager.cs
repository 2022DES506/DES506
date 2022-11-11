using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager SM;

    [SerializeField, Header("Main Audio Source")]
    private AudioSource audioSource;
    [SerializeField, Header("BGM Audio Source")]
    private AudioSource bgmAudioSource; 
    [Header("AudioClips")]
    [SerializeField]
    private AudioClip jump, pickup, trampoline, key;
    [SerializeField]
    private AudioClip Slowdown, portal;
    [SerializeField]
    private AudioClip backgroundMusic; 

    private void OnEnable()
    {
        if (SoundManager.SM == null)
        {
            SoundManager.SM = this; 
        }
    }

    public void PlayBGM()
    {
        bgmAudioSource.clip = backgroundMusic;
        bgmAudioSource.Play(); 
    }

    public void PlayJump()
    {
        audioSource.clip = jump;
        audioSource.Play(); 
    }

    public void PlaySpeedUp()
    {
        audioSource.clip = pickup;
        audioSource.Play();
    }

    public void PlayTrampoline()
    {
        audioSource.clip = trampoline;
        audioSource.Play();
    }

    public void PlayKeyGrab()
    {
        audioSource.clip = key;
        audioSource.Play();
    }

    public void PlaySlowdown()
    {
        audioSource.clip = Slowdown;
        audioSource.Play();
    }

    public void PlayPortal()
    {
        audioSource.clip = portal;
        audioSource.Play();
    }
}
