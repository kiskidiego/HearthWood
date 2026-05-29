using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(HealthComponent))]
public class PlayerHealthPostProcessing : MonoBehaviour
{
    [SerializeField] private float _minHPGreenBlue = 0.5f;
    [SerializeField] private float _fullHPVignette = 0f;
    [SerializeField] private float _minHPVignette = 0.5f;
    
    private Volume _postProcessingVolume;
    private ColorAdjustments _colorAdjustments;
    private Vignette _vignette;
    private HealthComponent _healthComponent;

    private void Awake()
    {
        _postProcessingVolume = FindAnyObjectByType<Volume>();
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnHealthChanged += UpdatePostProcessing;

        if (!_postProcessingVolume.profile.TryGet(out _colorAdjustments))
        {
            Debug.LogError("Color Adjustments not found in the post-processing profile.");
        }

        if (!_postProcessingVolume.profile.TryGet(out _vignette))
        {
            Debug.LogError("Vignette not found in the post-processing profile.");
        }
    }

    private void UpdatePostProcessing(HealthComponent.HealthChangeInfo info)
    {
        float healthPercentage = info.CurrentHealth / info.MaxHealth;
        // Update green and blue channels
        _colorAdjustments.colorFilter.Interp(Color.white, new Color32(255, (byte)(_minHPGreenBlue * 255), (byte)(_minHPGreenBlue * 255), 255), 1 - healthPercentage);

        // Update vignette intensity
        _vignette.intensity.value = Mathf.Lerp(_minHPVignette, _fullHPVignette, healthPercentage);
    }
}
