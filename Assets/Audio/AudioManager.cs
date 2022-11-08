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
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            speakers.Add(this.transform.GetChild(i).GetComponent<AudioSource>());
        }
    }

    public void PlayAudioOnSpeaker(AudioClip clip)
    {
        foreach(AudioSource source in speakers)
        {
            source.PlayOneShot(clip);
        }
    }

    public void PlayAudioOnPlayer(AudioClip clip)
    {
        playerAudioSource.PlayOneShot(clip);
    }

    private void Update()
    {
        if (Input.GetKeyDown(UnityEngine.KeyCode.A))
        {
            Debug.Log("je suis tout puissant !!!!!!!!!!!!!!!!");
            PlayAudioOnSpeaker(clip);
        }
    }
}
