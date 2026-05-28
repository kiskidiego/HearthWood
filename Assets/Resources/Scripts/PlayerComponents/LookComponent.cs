using UnityEngine;

public class LookComponent : MonoBehaviour
{
    [SerializeField] private float _lookSpeed = 100f;
    [SerializeField] private float _maxVerticalAngle = 80f;
    [SerializeField] private Transform _verticalPivot;
    private bool _canLook = true;
    public bool CanLook {
        get {return _canLook;}
        set {_canLook = value;}
    }

    public void Look(Vector2 lookInput)
    {
        if (!_canLook) return;

        float mouseX = lookInput.x * _lookSpeed * Time.deltaTime;
        float mouseY = lookInput.y * _lookSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        float currentVerticalAngle = _verticalPivot.localEulerAngles.x;
        currentVerticalAngle = (currentVerticalAngle > 180) ? currentVerticalAngle - 360 : currentVerticalAngle;
        float desiredVerticalAngle = Mathf.Clamp(currentVerticalAngle - mouseY, -_maxVerticalAngle, _maxVerticalAngle);
        _verticalPivot.localRotation = Quaternion.Euler(desiredVerticalAngle, 0f, 0f);
    }
}