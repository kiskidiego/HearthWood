using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CarriablePhysics))]
[RequireComponent(typeof(Interactable))]
public class Carriable : MonoBehaviour
{
    [SerializeField] string _notCarriedLayer = "Interactable";
    [SerializeField] string _carriedLayer = "Carried";

    CarriablePhysics _carryPhysicsComponent;
    Interactable _interactable;

    bool _isCarried = false;

    public bool IsCarried => _isCarried;

    void Awake()
    {
        _carryPhysicsComponent = GetComponent<CarriablePhysics>();
        _interactable = GetComponent<Interactable>();
    }

    void OnEnable()
    {
        _interactable.OnInteract += StartCarrying;
    }

    void OnDisable()
    {
        _interactable.OnInteract -= StartCarrying;
    }

    public void StartCarrying(GameObject interactor)
    {
        interactor.GetComponent<CarryComponent>()?.SetCarriedObject(this);
        _carryPhysicsComponent.StartCarrying(interactor);
        gameObject.layer = LayerMask.NameToLayer(_carriedLayer);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(_carriedLayer);
        }
        _isCarried = true;
    }
    public void StopCarrying()
    {
        _carryPhysicsComponent.StopCarrying();
        gameObject.layer = LayerMask.NameToLayer(_notCarriedLayer);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(_notCarriedLayer);
        }
        _isCarried = false;
    }

    void OnDestroy()
    {
        if (_isCarried)
        {
            StopCarrying();
        }
    }
}