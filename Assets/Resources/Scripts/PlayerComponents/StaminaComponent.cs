using UnityEngine;

public class StaminaComponent : MonoBehaviour
{
    [SerializeField] private float _maxStamina = 100f;
    [SerializeField] private float _staminaRegenRate = 5f;

    private float _currentStamina;
    private int _staminaRegenDisableCounter = 0;

    void Awake()
    {
        _currentStamina = _maxStamina;
    }

    void Update()
    {
        RegenStamina();
    }

    void RegenStamina()
    {
        if (_staminaRegenDisableCounter > 0) return;

        _currentStamina = Mathf.Clamp(_currentStamina + _staminaRegenRate * Time.deltaTime, 0, _maxStamina);
    }

    public bool HasEnoughStamina(float amount)
    {
        return _currentStamina >= amount;
    }

    public void ConsumeStamina(float amount)
    {
        _currentStamina = Mathf.Clamp(_currentStamina - amount, 0, _maxStamina);
    }

    public void DisableStaminaRegen(float duration = -1f)
    {
        _staminaRegenDisableCounter += 1;
        if (duration > 0)
        {
            Invoke(nameof(EnableStaminaRegenInternal), duration);
        }
    }

    private void EnableStaminaRegenInternal()
    {
        _staminaRegenDisableCounter -= 1;
    }

    public void EnableStaminaRegen(float delay = -1f)
    {
        if (delay > 0)
        {
            Invoke(nameof(EnableStaminaRegenInternal), delay);
        }
        else
        {
            EnableStaminaRegenInternal();
        }
    }
}
