using UnityEngine;

[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(StaminaComponent))]
public class SprintComponent : MonoBehaviour
{
    [SerializeField] private float _sprintMultiplier = 1.5f;
    [SerializeField] private float _staminaCostPerSecond = 10f;
    [SerializeField] private float _staminaRegenDelay = 2f;
    [SerializeField] private float _minStaminaToSprint = 20f;

    private MovementComponent _movementComponent;
    private GroundChecker _groundChecker;
    private StaminaComponent _staminaComponent;
    private bool _isSprinting = false;
    private bool _canSprint = true;
    public bool CanSprint {
        get {return _canSprint;}
        set 
        {
            _canSprint = value;
            if (!_canSprint && _isSprinting) StopSprint();
        }
    }

    void Awake()
    {
        _movementComponent = GetComponent<MovementComponent>();
        _groundChecker = GetComponent<GroundChecker>();
        _staminaComponent = GetComponent<StaminaComponent>();
    }

    void Update()
    {
        if (_isSprinting)
        {
            if (!_staminaComponent.HasEnoughStamina(_staminaCostPerSecond * Time.deltaTime))
            {
                StopSprint();
                return;
            }
            _staminaComponent.ConsumeStamina(_staminaCostPerSecond * Time.deltaTime);
        }
    }

    public void StartSprint()
    {
        if (!_canSprint || _isSprinting || !_groundChecker.IsGrounded()) return;

        if (!_staminaComponent.HasEnoughStamina(_minStaminaToSprint)) return;

        _staminaComponent.DisableStaminaRegen();
        _isSprinting = true;
        _movementComponent.MaxMovementSpeed *= _sprintMultiplier;
        _movementComponent.Acceleration *= _sprintMultiplier;
        _movementComponent.Deceleration *= _sprintMultiplier;
    }
    public void StopSprint()
    {
        if (!_isSprinting) return;

        _staminaComponent.EnableStaminaRegen(_staminaRegenDelay);
        _isSprinting = false;
        _movementComponent.MaxMovementSpeed /= _sprintMultiplier;
        _movementComponent.Acceleration /= _sprintMultiplier;
        _movementComponent.Deceleration /= _sprintMultiplier;
    }
}