using UnityEngine;

[RequireComponent(typeof(Fire))]
public class FireRefueler : MonoBehaviour
{
    [SerializeField] float _intensityPerLog = 0.5f;
    private Fire _fire;
    void Awake()
    {
        _fire = GetComponent<Fire>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            _fire.AddIntensity(_intensityPerLog);
            Destroy(other.gameObject);
        }
    }
}