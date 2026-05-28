using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(Collider))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float _maxMovementSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 15f;
    [SerializeField] private float _airControlMultiplier = 0.5f;

    public float MaxMovementSpeed {
        get {return _maxMovementSpeed;}
        set {_maxMovementSpeed = value;}
    }
    public float Acceleration {
        get {return _acceleration;}
        set {_acceleration = value;}
    }
    public float Deceleration {
        get {return _deceleration;}
        set {_deceleration = value;}
    }
    
    private bool canMove = true;
    public bool CanMove {
        get {return canMove;}
        set {canMove = value;}
    }
    private Rigidbody _rigidbody;
    private GroundChecker _groundChecker;
    private Vector2 _movementInput;
    private Collider _collider;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();
        _collider = GetComponent<Collider>();
    }

    public void SetMovementInput(Vector2 movementInput)
    {
        this._movementInput = movementInput;
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        if(_movementInput.sqrMagnitude < 0.01f && _groundChecker.IsGrounded())
        {
            _collider.material.dynamicFriction = 1f;
        }
        else
        {
            _collider.material.dynamicFriction = 0f;
        }

        Vector3 currentVelocity = _rigidbody.linearVelocity;
        Vector3 targetFoward = transform.forward * _movementInput.y;
        Vector3 targetRight = transform.right * _movementInput.x;
        Vector3 targetVelocity = (targetFoward + targetRight) * _maxMovementSpeed;

        float accelerationToUse = Vector3.Dot(currentVelocity, targetVelocity) <= 0f ? _deceleration : _acceleration;

        if (!_groundChecker.IsGrounded())
        {
            accelerationToUse *= _airControlMultiplier;
        }

        Vector3 newVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, accelerationToUse * Time.fixedDeltaTime);
        if (!_groundChecker.IsGrounded())
        {
            newVelocity.y = currentVelocity.y; // Preserve vertical velocity (gravity/falling)
        }

        _rigidbody.linearVelocity = newVelocity;
    }
}