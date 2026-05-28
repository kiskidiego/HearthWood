using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;

    void OnTriggerStay(Collider other)
    {
        HealthComponent healthComponent = other.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(_damage);
        }
    }
}
