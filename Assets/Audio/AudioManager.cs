using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public AudioManager instance { get; set; }
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip clip;
   


    private void Awake()
    {
        instance = this;
        PlayVFXAudio(clip);
    }


    public void PlayVFXAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();

    }
}
