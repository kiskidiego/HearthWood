using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    [SerializeField] private float _interactionRange = 3f;
    [SerializeField] private float _interactionSphereRadius = 0.1f;
    [SerializeField] private LayerMask _interactableLayerMask;
    private Camera _playerCamera;

    void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
    }

    public void Interact()
    {
        Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
        List<RaycastHit> hits = Physics.SphereCastAll(ray, _interactionSphereRadius, _interactionRange, _interactableLayerMask).ToList();

        hits.Sort((a, b) =>
        {
            return Vector3.Distance(transform.position, a.point) > Vector3.Distance(transform.position, b.point) ? 1 : -1;
        });

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == gameObject) continue; // Skip self

            Interactable interactable = hit.collider.GetComponent<Interactable>();
            
            if (interactable == null)
            {
                interactable = hit.collider.GetComponentInParent<Interactable>();
            }

            if (interactable != null)
            {
                interactable.Interact(gameObject);
                break; // Interact with the first valid interactable hit
            }
        }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (_playerCamera == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_playerCamera.transform.position, _playerCamera.transform.position + _playerCamera.transform.forward * _interactionRange);
        Gizmos.DrawWireSphere(_playerCamera.transform.position + _playerCamera.transform.forward * _interactionRange, _interactionSphereRadius);
    }
    #endif
}