using UnityEngine;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Fire))]
public class BonfireInteractionComponent : MonoBehaviour
{
    [SerializeField] float _rechargeIntensity = 2f;
    Interactable _interactable;
    Fire _fire;

    void Awake()
    {
        _interactable = GetComponent<Interactable>();
        _fire = GetComponent<Fire>();
    }

    void OnEnable()
    {
        _interactable.OnInteract += HandleInteraction;
    }
    void OnDisable()
    {
        _interactable.OnInteract -= HandleInteraction;
    }

    void HandleInteraction(GameObject interactor)
    {
        Fire fire = interactor.GetComponent<Fire>();
        if (fire != null && fire.GetIntensity() < 1.0f && _fire.GetIntensity() > 0f)
        {
            fire.SetIntensity(_rechargeIntensity); // Add intensity when interacted
        }
    }
}