using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpeaker : MonoBehaviour
{
    public static AudioSpeaker instance;

    [Header("Timer")]
    [SerializeField] private Speaker[] speaker;
    [Header("Timer")]
    [SerializeField] private AudioClip starterClip;
    [SerializeField] private AudioClip warningClip;
    [SerializeField] private AudioClip baseClip;
    [SerializeField] private AudioClip accelerateClip;
    [SerializeField] private AudioClip accelerateFasterClip;

    [Header("Alarme")]
    [SerializeField] private AudioClip softAlarmClip;
    [SerializeField] private AudioClip mediumAlarmClip;
    [SerializeField] private AudioClip hardAlarmClip;
    private int intensiteAlarm;


    private bool isAccelerate = false;
    private bool isAccelerateFaster = false;


    private void Start()
    {
        instance = this;

        speaker = this.GetComponentsInChildren<Speaker>();
        intensiteAlarm = 0;

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

    public void AlarmIntensite()
    {

        Debug.Log("Launch Alarm");
        if(intensiteAlarm == 0)
        {
            ApplyNewClipAlarm(softAlarmClip);
            intensiteAlarm++;
        }
        else if(intensiteAlarm == 1)
        {
            ApplyNewClipAlarm(mediumAlarmClip);
            intensiteAlarm++;
        }
        else if (intensiteAlarm == 2)
        {
            ApplyNewClipAlarm(mediumAlarmClip);
            intensiteAlarm++;
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

    private void ApplyNewClipAlarm(AudioClip clip)
    {
        foreach (Speaker actualSpeaker in speaker)
        {
            actualSpeaker.alarmSource.loop = true;
            actualSpeaker.alarmSource.clip = clip;
            actualSpeaker.alarmSource.Play();
        }
    }

    public void PauseAudio()
    {
        foreach (Speaker actualSpeaker in speaker)
        {
            actualSpeaker.timerSource.Pause();
            actualSpeaker.alarmSource.Pause();
        }
    }

    public IEnumerator StopAfter(int duration)
    {
        yield return new WaitForSeconds(duration);
        PauseAudio();
    }
}
