using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(HealthComponent))]
public class HurtSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] _hurtClips;
    private AudioSource _audioSource;
    private HealthComponent _healthComponent;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _healthComponent = GetComponent<HealthComponent>();

        _healthComponent.OnHealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(HealthComponent.HealthChangeInfo changeInfo)
    {
        if (_hurtClips.Length == 0) return;
        if (changeInfo.ChangeAmount >= 0) return;

        int randomIndex = Random.Range(0, _hurtClips.Length);
        _audioSource.PlayOneShot(_hurtClips[randomIndex]);
    }
}
