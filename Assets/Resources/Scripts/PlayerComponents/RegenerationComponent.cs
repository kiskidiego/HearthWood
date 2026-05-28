using System.Collections;
using UnityEngine;

public class RegenerationComponent : MonoBehaviour
{
    [SerializeField] private float _regenerationRate = 10f;
    [SerializeField] private float _regenerationDelay = 3f;
    private HealthComponent _healthComponent;

    void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
    }

    void OnEnable()
    {
        _healthComponent.OnHealthChanged += OnHealthChanged;
    }

    void OnDisable()
    {
        _healthComponent.OnHealthChanged -= OnHealthChanged;
    }

    void OnHealthChanged(HealthComponent.HealthChangeInfo info)
    {
        if (info.ChangeAmount < 0) // Only start regeneration if damage was taken
        {
            StopAllCoroutines();
            StartCoroutine(StartRegeneration());
        }
        else if (info.CurrentHealth >= info.MaxHealth) // Stop regeneration if health is full
        {
            StopAllCoroutines();
        }
    }

    IEnumerator StartRegeneration()
    {
        yield return new WaitForSeconds(_regenerationDelay);
        while (true)
        {
            _healthComponent.Heal(_regenerationRate * Time.deltaTime);
            yield return null;
        }
    }
}