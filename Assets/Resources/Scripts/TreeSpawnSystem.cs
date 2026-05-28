using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(TerrainSampler))]
[RequireComponent(typeof(Terrain))]
public class TreeSpawnSystem : MonoBehaviour
{
    public static TreeSpawnSystem Instance { get; private set; }
    [SerializeField] GameObject player;
    [SerializeField] GameObject _treePrefab;
    [SerializeField] int _maxTrees = 200;
    [SerializeField] float _minSpawnHeight = 100f;
    [SerializeField] float _minDistanceFromOtherTrees = 5f;
    [SerializeField] float _minDistanceFromPlayer = 10f;
    [SerializeField] float _spawnCooldown = 5f;
    [SerializeField] int _maxSpawnAttempts = 10;
    [SerializeField] TerrainLayer _treeTerrainLayer;
    TerrainSampler _terrainSampler;
    Terrain _terrain;
    int _currentTreeCount = 0;

    void Awake()
    {
        Instance = this;
        _terrainSampler = GetComponent<TerrainSampler>();
        _terrain = GetComponent<Terrain>();
    }

    void Start()
    {
        while (_currentTreeCount < _maxTrees)
        {
            if (!TryGetValidSpawnPosition(out Vector3 spawnPosition)) continue;

            Instantiate(_treePrefab, spawnPosition, Quaternion.identity);
            _currentTreeCount++;
            Debug.Log("Spawned tree at: " + spawnPosition + " Current tree count: " + _currentTreeCount);
        }

        StartCoroutine(SpawnTrees());
    }

    IEnumerator SpawnTrees()
    {
        while (true)
        {
            if (_currentTreeCount < _maxTrees)
            {
                if (TryGetValidSpawnPosition(out Vector3 spawnPosition))
                {
                    Instantiate(_treePrefab, spawnPosition, Quaternion.identity);
                    _currentTreeCount++;
                }
            }
            yield return new WaitForSeconds(_spawnCooldown);
        }
    }

    bool TryGetValidSpawnPosition(out Vector3 spawnPosition)
    {
        for (int i = 0; i < _maxSpawnAttempts; i++) // Try _maxSpawnAttempts times to find a valid position
        {
            float x = Random.Range(_terrain.terrainData.size.x * 0.1f, _terrain.terrainData.size.x * 0.9f);
            float z = Random.Range(_terrain.terrainData.size.z * 0.1f, _terrain.terrainData.size.z * 0.9f);
            RaycastHit hit;
            Vector3 origin = new Vector3(x, _terrain.terrainData.size.y, z);
            if (Physics.Raycast(origin, Vector3.down, out hit, _terrain.terrainData.size.y - _minSpawnHeight))
            {
                if (hit.point.y >= _minSpawnHeight)
                {
                    Terrain terrain = hit.collider.GetComponent<Terrain>();
                    if (terrain == null) continue; // Not terrain, skip

                    if (!CheckPlayerProximity(hit.point)) continue; // Too close to player, skip

                    if (!CheckTerrain(hit.point)) continue; // Terrain check failed, skip

                    if (!CheckTreeProximity(hit.point)) continue; // Too close to another tree, skip

                    spawnPosition = hit.point;
                    return true;
                }
            }
        }
        spawnPosition = Vector3.zero;
        return false;
    }

    bool CheckTerrain(Vector3 hitPoint)
    {
        TerrainLayer terrainLayerAtPosition = _terrainSampler.GetTerrainLayerAtPosition(hitPoint);
        return terrainLayerAtPosition == _treeTerrainLayer;
    }

    bool CheckPlayerProximity(Vector3 position)
    {
        float distanceToPlayer = Vector3.Distance(position, player.transform.position);
        return distanceToPlayer >= _minDistanceFromPlayer;
    }

    bool CheckTreeProximity(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, _minDistanceFromOtherTrees);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Tree"))
            {
                return false; // Too close to another tree
            }
        }
        return true;
    }

    public void OnTreeDestroyed()
    {
        _currentTreeCount--;
    }
}