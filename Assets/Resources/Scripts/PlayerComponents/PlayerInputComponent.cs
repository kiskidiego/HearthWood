using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(LookComponent))]
[RequireComponent(typeof(SprintComponent))]
[RequireComponent(typeof(JumpComponent))]
[RequireComponent(typeof(InteractComponent))]
[RequireComponent(typeof(CarryComponent))]
public class PlayerInputComponent : MonoBehaviour
{
    [SerializeField] private InputActionReference _movementInput;
    [SerializeField] private InputActionReference _lookInput;
    [SerializeField] private InputActionReference _sprintInput;
    [SerializeField] private InputActionReference _jumpInput;
    [SerializeField] private InputActionReference _interactInput;
    [SerializeField] private InputActionReference _dropInput;
    [SerializeField] private InputActionReference _throwInput;
    private MovementComponent _movementComponent;
    private LookComponent _lookComponent;
    private SprintComponent _sprintComponent;
    private JumpComponent _jumpComponent;
    private InteractComponent _interactComponent;
    private CarryComponent _carryComponent;

    private bool canReceiveInput = true;
    public bool CanReceiveInput {
        get {return canReceiveInput;}
        set {canReceiveInput = value;}
    }

    void Awake()
    {
        CaptureMouse();
        
        _movementComponent = GetComponent<MovementComponent>();
        _lookComponent = GetComponent<LookComponent>();
        _sprintComponent = GetComponent<SprintComponent>();
        _jumpComponent = GetComponent<JumpComponent>();
        _interactComponent = GetComponent<InteractComponent>();
        _carryComponent = GetComponent<CarryComponent>();
    }

    public void CaptureMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ReleaseMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnEnable()
    {
        _movementInput.action.Enable();
        _movementInput.action.performed += OnMovementInput;
        _movementInput.action.canceled += OnMovementInput;

        _lookInput.action.Enable();
        _lookInput.action.performed += OnLookInput;
        _lookInput.action.canceled += OnLookInput;

        _sprintInput.action.Enable();
        _sprintInput.action.performed += OnSprintInput;
        _sprintInput.action.canceled += OnSprintInput;

        _jumpInput.action.Enable();
        _jumpInput.action.performed += OnJumpInput;

        _interactInput.action.Enable();
        _interactInput.action.performed += OnInteractInput;

        _dropInput.action.Enable();
        _dropInput.action.performed += OnDropInput;

        _throwInput.action.Enable();
        _throwInput.action.performed += OnThrowInput;
    }

    void OnDisable()
    {
        _movementInput.action.Disable();
        _movementInput.action.performed -= OnMovementInput;
        _movementInput.action.canceled -= OnMovementInput;

        _lookInput.action.Disable();
        _lookInput.action.performed -= OnLookInput;
        _lookInput.action.canceled -= OnLookInput;

        _sprintInput.action.Disable();
        _sprintInput.action.performed -= OnSprintInput;
        _sprintInput.action.canceled -= OnSprintInput;

        _jumpInput.action.Disable();
        _jumpInput.action.performed -= OnJumpInput;

        _interactInput.action.Disable();
        _interactInput.action.performed -= OnInteractInput;

        _dropInput.action.Disable();
        _dropInput.action.performed -= OnDropInput;

        _throwInput.action.Disable();
        _throwInput.action.performed -= OnThrowInput;
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        if (!canReceiveInput) return;

        Vector2 movementInput = context.ReadValue<Vector2>();
        _movementComponent.SetMovementInput(movementInput);
    }

    void OnLookInput(InputAction.CallbackContext context)
    {
        if (!canReceiveInput) return;

        Vector2 lookInput = context.ReadValue<Vector2>();
        _lookComponent.OnLookInput(lookInput);
    }

    void OnSprintInput(InputAction.CallbackContext context)
    {
        if (!canReceiveInput) return;

        if (context.performed) _sprintComponent.SetTryingToSprint(true);
        else if (context.canceled) _sprintComponent.SetTryingToSprint(false);
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        if (!canReceiveInput) return;

        _jumpComponent.Jump();
    }

    void OnInteractInput(InputAction.CallbackContext context)
    {
        if (!canReceiveInput) return;

        _interactComponent.Interact();
    }

    void OnDropInput(InputAction.CallbackContext context)
    {
        if (!canReceiveInput) return;

        _carryComponent.DropCarriedObject();
    }

    void OnThrowInput(InputAction.CallbackContext context)
    {
        if (!canReceiveInput) return;

        _carryComponent.ThrowCarriedObject();
    }
}