using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager SM;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip jump, pickup, trampoline;

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
}
