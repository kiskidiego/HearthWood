using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public struct HealthChangeInfo
    {
        public float CurrentHealth;
        public float ChangeAmount;
        public float MaxHealth;

        public HealthChangeInfo(float currentHealth, float changeAmount, float maxHealth)
        {
            CurrentHealth = currentHealth;
            ChangeAmount = changeAmount;
            MaxHealth = maxHealth;
        }
    }

    public Action<HealthChangeInfo> OnHealthChanged = delegate { };
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _damageCooldown = 0.0f;
    private float _currentHealth;
    public Action OnDeath = delegate { };

    bool _canTakeDamage = true;
    public bool CanTakeDamage {
        get { return _canTakeDamage; }
        set { _canTakeDamage = value; }
    }

    void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!_canTakeDamage) return;

        _currentHealth -= damage;
        _canTakeDamage = false;
        Invoke(nameof(ResetDamageCooldown), _damageCooldown);
        OnHealthChanged?.Invoke(new HealthChangeInfo(_currentHealth, -damage, _maxHealth));
        if (_currentHealth <= 0)
        {
            Die();
        }
        Debug.Log($"Took {damage} damage, current health: {_currentHealth}");
    }

    void ResetDamageCooldown()
    {
        _canTakeDamage = true;
    }

    public void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        OnHealthChanged?.Invoke(new HealthChangeInfo(_currentHealth, amount, _maxHealth));
        Debug.Log($"Healed {amount} health, current health: {_currentHealth}");
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}