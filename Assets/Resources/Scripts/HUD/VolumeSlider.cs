using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerSlider : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private string _parameterName = "Volume";
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(SetVolume);
    }
    void Start()
    {
        if (PlayerPrefs.HasKey(_parameterName))
        {
            float savedVolume = PlayerPrefs.GetFloat(_parameterName);
            slider.value = savedVolume;
            SetVolume(savedVolume);
        }
    }
    public void SetVolume(float volume)
    {
        _audioMixerGroup.audioMixer.SetFloat(_parameterName, Mathf.Log10(Math.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat(_parameterName, volume);
    }
}
