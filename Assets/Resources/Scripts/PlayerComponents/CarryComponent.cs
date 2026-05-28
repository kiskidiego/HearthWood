using UnityEngine;

public class CarryComponent : Interactable
{
    [SerializeField] private float _throwForce = 10f;
    [SerializeField] private Carriable _carriedObject;
    Camera _playerCamera;

    void Awake()
    {
        if (_carriedObject != null)
        {
            _carriedObject.StartCarrying(gameObject);
        }
        _playerCamera = GetComponentInChildren<Camera>();
    }
    public void SetCarriedObject(Carriable carriable)
    {
        if (_carriedObject != null && _carriedObject != carriable)
        {
            _carriedObject.StopCarrying();
        }
        _carriedObject = carriable;
    }

    public void DropCarriedObject()
    {
        if (_carriedObject == null) return;

        _carriedObject.StopCarrying();
        _carriedObject = null;
    }
    public void ThrowCarriedObject()
    {
        if (_carriedObject == null) return;

        Rigidbody carriedRigidbody = _carriedObject.GetComponent<Rigidbody>();
        if (carriedRigidbody != null)
        {
            carriedRigidbody.AddForce(_playerCamera.transform.forward * _throwForce, ForceMode.VelocityChange);
        }

        DropCarriedObject();
    }
}