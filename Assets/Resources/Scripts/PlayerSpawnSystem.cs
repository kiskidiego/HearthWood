using System.Data.Common;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(TerrainSampler))]
public class PlayerSpawnSystem : MonoBehaviour
{
    [SerializeField] GameObject[] _playerObjects;
    [SerializeField] float _minSpawnHeight = 100f;
    [SerializeField] float _minDistanceBetweenPlayerObjects = 1.25f;
    [SerializeField] float _maxDistanceBetweenPlayerObjects = 5f;
    [SerializeField] int _attemptsPerPivot = 100;
    [SerializeField] TerrainLayer _spawnTerrainLayer;
    Terrain _terrain;
    TerrainSampler _terrainSampler;
    int _spawnedObjectsCount = 0;

    void Awake()
    {
        _terrain = GetComponent<Terrain>();
        _terrainSampler = GetComponent<TerrainSampler>();
    }

    void Start()
    {
        while (_spawnedObjectsCount < _playerObjects.Length)
        {
            _spawnedObjectsCount = 0;
            if (TryGetValidSpawnPosition(out Vector3 pivot))
            {
                for (int i = 0; i < _attemptsPerPivot && _spawnedObjectsCount < _playerObjects.Length; i++)
                {
                    Vector3 spawnPosition = pivot + Random.insideUnitSphere * _maxDistanceBetweenPlayerObjects * 0.5f;
                    
                    RaycastHit hit;
                    Vector3 origin = new Vector3(spawnPosition.x, _terrain.terrainData.size.y, spawnPosition.z);
                    if (Physics.Raycast(origin, Vector3.down, out hit, _terrain.terrainData.size.y - _minSpawnHeight))
                    {
                        spawnPosition.y = hit.point.y;
                    }
                    else
                    {
                        continue; // If the raycast doesn't hit, skip this attempt
                    }
                    Debug.Log("Attempting to spawn player object: " + _playerObjects[_spawnedObjectsCount].name + " at: " + spawnPosition);
                    if (IsValidSpawnPosition(spawnPosition))
                    {
                        GameObject playerObject = _playerObjects[_spawnedObjectsCount];
                        playerObject.transform.position = spawnPosition; // Slightly above the ground to prevent clipping
                        if(!playerObject.TryGetComponent<Rigidbody>(out _))
                            playerObject.transform.up = hit.normal; // Align the player object with the terrain normal
                        else
                            playerObject.transform.position += Vector3.up; // If it has a Rigidbody, just move it up a bit to prevent clipping
                        _spawnedObjectsCount++;
                    }
                    if (_spawnedObjectsCount >= _playerObjects.Length) break; // Break if we've spawned all player objects
                }
            }
        }
        Physics.SyncTransforms();
    }

    bool TryGetValidSpawnPosition(out Vector3 spawnPosition)
    {
        float x = Random.Range(_terrain.terrainData.size.x * 0.1f, _terrain.terrainData.size.x * 0.9f);
        float z = Random.Range(_terrain.terrainData.size.z * 0.1f, _terrain.terrainData.size.z * 0.9f);
        RaycastHit hit;
        Vector3 origin = new Vector3(x, _terrain.terrainData.size.y, z);
        if (Physics.Raycast(origin, Vector3.down, out hit, _terrain.terrainData.size.y - _minSpawnHeight))
        {
            foreach (GameObject playerObject in _playerObjects)
            {
                if (hit.collider.gameObject == playerObject) 
                {
                    spawnPosition = Vector3.zero;
                    return false;
                }
            }
            if (hit.point.y >= _minSpawnHeight && _terrainSampler.GetTerrainLayerAtPosition(hit.point) == _spawnTerrainLayer)
            {
                spawnPosition = hit.point;
                return true;
            }
        }
        spawnPosition = Vector3.zero;
        return false;
    }

    bool IsValidSpawnPosition(Vector3 position)
    {
        foreach (GameObject playerObject in _playerObjects)
        {
            if (Vector3.Distance(position, playerObject.transform.position) < _minDistanceBetweenPlayerObjects)
            {
                return false;
            }
        }
        return _terrainSampler.GetTerrainLayerAtPosition(position) == _spawnTerrainLayer;
    }
}