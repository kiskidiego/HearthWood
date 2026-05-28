using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Axe : MonoBehaviour
{
    [SerializeField] float _damagePerUnitForce = 1f;
    [SerializeField] float _minForceForDamage = 5f;
    Rigidbody _rigidbody;
    Carriable _carriableComponent;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carriableComponent = GetComponent<Carriable>();
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

        float otherMass = (collision.rigidbody != null) ? collision.rigidbody.mass : _rigidbody.mass;

        float myMass = _rigidbody.mass;

        float effectiveMass = (otherMass * myMass) / (otherMass + myMass);

        float relativeVelocity = collision.relativeVelocity.magnitude;

        float force = 0.5f * effectiveMass * relativeVelocity * relativeVelocity;

        if (force < _minForceForDamage) return;

        float damage = (force - _minForceForDamage) * _damagePerUnitForce;

        health.TakeDamage(damage);

        Debug.Log($"Axe hit {collision.collider.name} with force {force}, dealing {damage} damage.");
    }

}
