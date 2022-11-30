using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioManager instance { get; set; }

    [SerializeField]
    private AudioMixer audioMixer;

    private List<AudioSource> speakers = new List<AudioSource>();
    [SerializeField] private AudioSource playerAudioSource;

    [SerializeField] AudioClip clip;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    /*public void PlayAudioOnSpeaker(AudioClip clip)
    {
        foreach(AudioSource source in speakers)
        {
            source.PlayOneShot(clip);
        }
    }

    public void PlayAudioOnPlayer(AudioClip clip)
    {
        playerAudioSource.PlayOneShot(clip);
    }*/


    public static float ParseToDebit0(float value)
    {
        float parse = Mathf.Lerp(-80, 00, Mathf.Clamp01(value));
        return parse;
    }

    public static float ParseToDebit20(float value)
    {
        float parse = Mathf.Lerp(-80, 20, Mathf.Clamp01(value));
        return parse;
    }

    public static float ParseToDebitCustom(float value, float min = -80, float max = 20)
    {
        float parse = Mathf.Lerp(min, max, Mathf.Clamp01(value));
        return parse;
    }
}
