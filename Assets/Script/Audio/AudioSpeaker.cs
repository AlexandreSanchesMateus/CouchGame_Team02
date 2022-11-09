using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpeaker : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private Speaker[] speaker;
    [Header("Timer")]
    [SerializeField] private AudioClip starterClip;
    [SerializeField] private AudioClip warningClip;
    [SerializeField] private AudioClip baseClip;
    [SerializeField] private AudioClip accelerateClip;
    [SerializeField] private AudioClip accelerateFasterClip;

    [Header("Alarme")]

    private bool isAccelerate = false;
    private bool isAccelerateFaster = false;


    private void Start()
    {
        speaker = this.GetComponentsInChildren<Speaker>();

        StartCoroutine(ApplyNewClipTimer(starterClip, false, 0));
        StartCoroutine(ApplyNewClipTimer(baseClip, true, starterClip.length));
    }

    private void Update()
    {
        if(Mathf.Round(Timer.instance.timeRemaining) == 119 && !isAccelerate)
        {
            StartCoroutine(ApplyNewClipTimer(warningClip, false, 0));
            StartCoroutine(ApplyNewClipTimer(accelerateClip, true, warningClip.length));
            isAccelerate = true;
        }
        else if(Mathf.Round(Timer.instance.timeRemaining) == 59 && !isAccelerateFaster)
        {
            StartCoroutine(ApplyNewClipTimer(warningClip, false, 0));
            StartCoroutine(ApplyNewClipTimer(accelerateClip, true, warningClip.length));
            isAccelerate = true;
        }
    }

    private IEnumerator ApplyNewClipTimer(AudioClip clip, bool looped, float wait)
    {
        yield return new WaitForSeconds(wait);
        foreach (Speaker actualSpeaker in speaker)
        { 
            actualSpeaker.timerSource.loop = looped;
            actualSpeaker.timerSource.clip = clip;
            actualSpeaker.timerSource.Play();
        }
    }
}
