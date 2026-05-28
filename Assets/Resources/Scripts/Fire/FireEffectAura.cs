using UnityEngine;

[RequireComponent(typeof(Fire))]
public class FireEffectAura : MonoBehaviour
{
    [SerializeField] float _intensityRadiusScaling = 7.5f;
    [SerializeField] float _maxRadius = 10f;
    [SerializeField] float _minRadius = 3f;
    [SerializeField] SphereCollider _sphereCollider;
    private Fire _fire;

    void Awake()
    {
        _fire = GetComponent<Fire>();
        if (_sphereCollider == null)
        {
            _sphereCollider = GetComponent<SphereCollider>();
        }
    }

    void Update()
    {
        float intensity = _fire.GetIntensity();
        _sphereCollider.radius = Mathf.Clamp(intensity * _intensityRadiusScaling, _minRadius, _maxRadius);
        if (intensity <= 0f)
        {
            _sphereCollider.enabled = false;
        }
        else
        {
            _sphereCollider.enabled = true;
        }
    }
}