using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FireAudio : MonoBehaviour
{
    enum FireSoundState
    {
        None,
        Weak,
        Medium,
        Strong
    }
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioClip _strongFireClip;
    [SerializeField] private AudioClip _mediumFireClip;
    [SerializeField] private AudioClip _weakFireClip;
    [SerializeField] private float _maxIntensity = 1f;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _maxVolume = 1f;
    [SerializeField] private Fire _fire;


    private AudioSource _strongFireSound;
    private AudioSource _mediumFireSound;
    private AudioSource _weakFireSound;

    private AudioSource[] _audioSources;

    private FireSoundState _currentSoundState = FireSoundState.None;

    private Dictionary<AudioSource, Coroutine> _fadeCoroutines = new Dictionary<AudioSource, Coroutine>();

    private void Awake()
    {
        if (_fire == null)
        {
            _fire = GetComponent<Fire>();
        }

        _strongFireSound = gameObject.AddComponent<AudioSource>();
        _strongFireSound.clip = _strongFireClip;
        _mediumFireSound = gameObject.AddComponent<AudioSource>();
        _mediumFireSound.clip = _mediumFireClip;
        _weakFireSound = gameObject.AddComponent<AudioSource>();
        _weakFireSound.clip = _weakFireClip;

        _audioSources = new[] { _strongFireSound, _mediumFireSound, _weakFireSound };

        foreach (var source in _audioSources)
        {
            if (_audioMixerGroup != null)
            {
                source.outputAudioMixerGroup = _audioMixerGroup;
            }
            source.spatialBlend = 1f; // 3D sound
            source.volume = 0f;
            source.loop = true;
        }
    }

    private void Update()
    {
        float intensity = _fire.GetIntensity() / _maxIntensity;

        if (intensity > 0.66f)
        {
            if (_currentSoundState == FireSoundState.Strong) return;

            _currentSoundState = FireSoundState.Strong;
            StartCrossfade(_strongFireSound);
        }
        else if (intensity > 0.33f)
        {
            if (_currentSoundState == FireSoundState.Medium) return;

            _currentSoundState = FireSoundState.Medium;
            StartCrossfade(_mediumFireSound);
        }
        else if (intensity > 0f)
        {
            if (_currentSoundState == FireSoundState.Weak) return;

            _currentSoundState = FireSoundState.Weak;
            StartCrossfade(_weakFireSound);
        }
        else
        {
            if (_currentSoundState == FireSoundState.None) return;

            _currentSoundState = FireSoundState.None;
            StopAll();
        }
    }

    private void StartCrossfade(AudioSource targetSource)
    {
        Debug.Log($"Starting crossfade to {targetSource.clip.name}");

        if (!targetSource.isPlaying)
        {
            targetSource.Play();
        }

        
        foreach (var source in _audioSources)
        {
            if (_fadeCoroutines.TryGetValue(source, out var coroutine))
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
            }
            if (source == targetSource)
            {
                _fadeCoroutines[source] = StartCoroutine(FadeIn(source));
            }
            else
            {
                _fadeCoroutines[source] = StartCoroutine(FadeOut(source));
            }
        }
    }

    private void StopAll()
    {
        foreach (var source in _audioSources)
        {
            source.Stop();
            source.volume = 0f;
        }
    }

    private IEnumerator FadeIn(AudioSource source)
    {
        float targetVolume = _maxVolume;
        while (source.volume < targetVolume)
        {
            source.volume = Mathf.MoveTowards(source.volume, targetVolume, Time.deltaTime / _fadeDuration);
            yield return null;
        }
        source.volume = targetVolume;
    }

    private IEnumerator FadeOut(AudioSource source)
    {
        float targetVolume = 0f;
        while (source.volume > targetVolume)
        {
            source.volume = Mathf.MoveTowards(source.volume, targetVolume, Time.deltaTime / _fadeDuration);
            yield return null;
        }
        source.volume = targetVolume;
        source.Stop();
    }
}
