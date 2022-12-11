using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeSettings : MonoBehaviour
{
    [Header("MUSIC")]
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TMP_InputField musicField;

    [Header("SFX")]
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_InputField sfxField;

    [Header("AMBIENCE")]
    [SerializeField] private AudioMixer ambianceMixer;
    [SerializeField] private Slider ambienceSlider;
    [SerializeField] private TMP_InputField ambienceField;

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SfxVolume";
    const string MIXER_AMBIENCE = "AmbianceVolume";

    private void Awake()
    {
        // Fields
        musicField.onSubmit.AddListener(SubmitMusicField);
        sfxField.onSubmit.AddListener(SubmitSfxField);
        ambienceField.onSubmit.AddListener(SubmitAmbienceField);

        // Sliders
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        ambienceSlider.onValueChanged.AddListener(SetAmbienceVolume);
    }

    private void Start()
    {
        if (musicMixer.GetFloat(MIXER_MUSIC, out float music))
        {
            musicSlider.value = (music + 80) / 100;
        }

        if (sfxMixer.GetFloat(MIXER_SFX, out float sfx))
        {
            sfxSlider.value = (sfx + 80) / 100;
        }

        if (ambianceMixer.GetFloat(MIXER_AMBIENCE, out float ambience))
        {
            ambienceSlider.value = (ambience + 80) / 100;
        }
    }

    private void SubmitMusicField(string value)
    {
        if (float.TryParse(value, out float submit))
            musicSlider.value = Mathf.Clamp(submit, 0, 100) / 100.0f;
        else
            musicField.text = (musicSlider.value * 100).ToString();
    }

    private void SubmitSfxField(string value)
    {
        if (float.TryParse(value, out float submit))
            sfxSlider.value = Mathf.Clamp(submit, 0, 100) / 100.0f;
        else
            sfxField.text = (sfxSlider.value * 100).ToString();
    }

    private void SubmitAmbienceField(string value)
    {
        if (float.TryParse(value, out float submit))
            ambienceSlider.value = Mathf.Clamp(submit, 0, 100) / 100.0f;
        else
            ambienceField.text = (ambienceSlider.value * 100).ToString();
    }

    private void SetMusicVolume(float value)
    {
        musicMixer.SetFloat(MIXER_MUSIC, ParseToDebit20(value));
        musicField.text = ((int)(value * 100)).ToString();
    }

    private void SetSfxVolume(float value)
    {
        sfxMixer.SetFloat(MIXER_SFX, ParseToDebit20(value));
        sfxField.text = (value * 100).ToString();
    }

    private void SetAmbienceVolume(float value)
    {
        ambianceMixer.SetFloat(MIXER_AMBIENCE, ParseToDebit20(value));
        ambienceField.text = (value * 100).ToString();
    }

    private static float ParseToDebit20(float value)
    {
        float parse = Mathf.Lerp(-80, 20, Mathf.Clamp01(value));
        return parse;
    }
}
