using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(StaminaComponent))]
public class JumpComponent : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _staminaCost = 20f;
    [SerializeField] private float _staminaRegenDelay = 1f;
    private Rigidbody _rigidbody;
    private GroundChecker _groundChecker;
    private StaminaComponent _staminaComponent;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();
        _staminaComponent = GetComponent<StaminaComponent>();
    }

    public void Jump()
    {
        if (!_groundChecker.IsGrounded()) return;

        if (!_staminaComponent.HasEnoughStamina(_staminaCost)) return;

        _staminaComponent.ConsumeStamina(_staminaCost);
        _staminaComponent.DisableStaminaRegen(_staminaRegenDelay);

        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
}