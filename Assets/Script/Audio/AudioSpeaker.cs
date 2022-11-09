using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpeaker : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private AudioSource[] audioSource;
    [SerializeField] private AudioClip starterClip;
    [SerializeField] private AudioClip warningClip;
    [SerializeField] private AudioClip baseClip;
    [SerializeField] private AudioClip accelerateClip;

    private bool isAccelerate = false;


    private void Start()
    {
        audioSource = this.GetComponentsInChildren<AudioSource>();

        StartCoroutine(ApplyNewClip(starterClip, false, 0));
        StartCoroutine(ApplyNewClip(baseClip, true, starterClip.length));
    }

    private void Update()
    {
        if(Mathf.Round(Timer.instance.timeRemaining) == 119 && !isAccelerate)
        {
            StartCoroutine(ApplyNewClip(warningClip, false, 0));
            StartCoroutine(ApplyNewClip(accelerateClip, true, warningClip.length));
            isAccelerate = true;
        }
    }

    private IEnumerator ApplyNewClip(AudioClip clip, bool looped, float wait)
    {
        yield return new WaitForSeconds(wait);
        foreach (AudioSource audioSource in audioSource)
        { 
            audioSource.loop = looped;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
