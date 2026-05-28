using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(PlayerInputComponent))]
public class PlayerDeathComponent : MonoBehaviour
{
    [SerializeField] float _deathHeight = 0f;
    private HealthComponent _healthComponent;
    private PlayerInputComponent _inputComponent;

    void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _inputComponent = GetComponent<PlayerInputComponent>();
    }

    void OnEnable()
    {
        _healthComponent.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        _healthComponent.OnDeath -= HandleDeath;
    }

    void Update()
    {
        if (transform.position.y < _deathHeight)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        _inputComponent.CanReceiveInput = false;
        GameManager.Instance?.Lose();
    }
}