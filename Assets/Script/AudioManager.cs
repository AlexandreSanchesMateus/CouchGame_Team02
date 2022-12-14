using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; set; }

    [SerializeField] private AudioMixer music_audioMixer;
    [SerializeField] private AudioMixer ambiance_audioMixer;
    [SerializeField, Range (0, 5)] private float transitionTime = 0.3f;

    int level = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void ChangeAmbiance(AudioMixerSnapshot snapshot)
    {
        snapshot.TransitionTo(transitionTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(UnityEngine.KeyCode.M))
        {
            ReduceMusicLevel();
        }

        if (Input.GetKeyDown(UnityEngine.KeyCode.P))
        {
            IncreaseMusicLevel();
        }
    }

    public void IncreaseMusicLevel()
    {
        level++;
        ChangeMusicLevel(level);
    }

    public void ReduceMusicLevel()
    {
        level--;
        ChangeMusicLevel(level);
    }

    public void ChangeMusicLevel(int level)
    {
        level = Mathf.Clamp(level, 1, 10);
        music_audioMixer.FindSnapshot("LEVEL_" + level.ToString()).TransitionTo(0.3f);
    }

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
