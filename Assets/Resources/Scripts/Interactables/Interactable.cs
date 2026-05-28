using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Action<GameObject> OnInteract = delegate { };
    public void Interact(GameObject interactor)
    {
        Debug.Log("Interacted with " + gameObject.name);
        OnInteract?.Invoke(interactor);
    }
}
