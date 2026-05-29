using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class Axe : MonoBehaviour
{
    [SerializeField] float _damagePerUnitForce = 1f;
    [SerializeField] float _minForceForDamage = 5f;
    [SerializeField] private AudioClip[] _hitSounds;
    [SerializeField] float _maxDamagoForVolume = 30f;
    AudioSource _audioSource;
    Rigidbody _rigidbody;
    Carriable _carriableComponent;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carriableComponent = GetComponent<Carriable>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!_carriableComponent.IsCarried) return;
        if (collision.collider.CompareTag("Player")) return;

        HealthComponent health = collision.collider.GetComponent<HealthComponent>();

        if (health == null)
        {
            health = collision.collider.GetComponentInParent<HealthComponent>();
        }

        if (health == null) return;

        float relativeVelocity = collision.relativeVelocity.magnitude;
        float force = 0.5f * relativeVelocity * relativeVelocity; // Assuming mass = 1 for simplicity, otherwise use 0.5 * mass * velocity^2

        if (force < _minForceForDamage) return;

        float damage = (force - _minForceForDamage) * _damagePerUnitForce;

        health.TakeDamage(damage);

        float volume = Mathf.Pow(Mathf.Clamp01(damage / _maxDamagoForVolume), 2);
        _audioSource.PlayOneShot(_hitSounds[Random.Range(0, _hitSounds.Length)], volume);
    }

}
