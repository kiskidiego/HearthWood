using System;
using UnityEngine;
using UnityEngine.Events;

public class Fire : MonoBehaviour
{
    public UnityEvent OnFireExtinguished = new UnityEvent();
    [SerializeField] float _decayRate = 0.05f;
    [SerializeField] float _maxParticleIntensity = 8f;
    [SerializeField] float _minParticleIntensity = 0.3f;
    [SerializeField] float _lightIntensityMultiplier = 10f;
    [SerializeField] float _initialIntensity = 4f;
    float _intensity = 4f;
    private ParticleSystem _fireParticleSystem;
    private Light _fireLight;
    void Start()
    {
        _fireParticleSystem = GetComponentInChildren<ParticleSystem>();
        _fireLight = GetComponentInChildren<Light>();
        _intensity = _initialIntensity;
    }
    void Update()
    {
        if (_intensity <= 0) return;

        _intensity -= _decayRate * Time.deltaTime;
        var main = _fireParticleSystem.main;
        main.startLifetime = Mathf.Clamp(_intensity, _minParticleIntensity, _maxParticleIntensity);
        _fireLight.intensity = _intensity * _lightIntensityMultiplier;

        if (_intensity <= 0)
        {
            _intensity = 0;
            _fireParticleSystem.Stop();
            OnFireExtinguished?.Invoke();
        }
    }

    public void AddIntensity(float amount)
    {
        _intensity += amount;
        if (_intensity > 0)
        {
            _fireParticleSystem.Play();
        }
    }

    public void SetIntensity(float intensity)
    {
        _intensity = intensity;
        if (_intensity > 0)
        {
            _fireParticleSystem.Play();
        }
        else
        {
            _fireParticleSystem.Stop();
        }
    }

    public float GetIntensity()
    {
        return _intensity;
    }
}
