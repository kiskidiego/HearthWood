using System;
using UnityEngine;

public class RandomMesh : MonoBehaviour
{
    [SerializeField] GameObject[] _possibleMeshes;

    void Awake()
    {
        if (_possibleMeshes.Length == 0)
        {
            Debug.LogWarning("No meshes assigned to RandomMesh component on " + gameObject.name);
            return;
        }
        int randomIndex = UnityEngine.Random.Range(0, _possibleMeshes.Length);
        GameObject selectedMesh = _possibleMeshes[randomIndex];
        foreach (GameObject mesh in _possibleMeshes)
        {
            mesh.SetActive(mesh == selectedMesh);
        }
    }
}