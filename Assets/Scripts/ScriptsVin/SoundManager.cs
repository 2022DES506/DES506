using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager SM;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip jump, pickup, trampoline, key;
    [SerializeField]
    private AudioClip Slowdown, boost;

    private void OnEnable()
    {
        if (SoundManager.SM == null)
        {
            SoundManager.SM = this; 
        }
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

    public void PlayBoost()
    {
        audioSource.clip = boost;
        audioSource.Play();
    }
}
