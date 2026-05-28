using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class Tree : MonoBehaviour
{
    [SerializeField] GameObject _logPrefab;
    [SerializeField] Vector2 _logsToSpawn = new Vector2(1, 3);
    [SerializeField] float _spawnHeight = 3f;
    [SerializeField] float _spawnRadius = 0.5f;
    
    HealthComponent _healthComponent;

    void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnDeath += HandleTreeDeath;
    }

    void HandleTreeDeath()
    {
        int logsToSpawn = Random.Range((int)_logsToSpawn.x, (int)_logsToSpawn.y + 1);
        for (int i = 0; i < logsToSpawn; i++)
        {
            Instantiate(_logPrefab, transform.position + Vector3.up * _spawnHeight + Random.insideUnitSphere * _spawnRadius, Quaternion.identity);
        }
        Destroy(gameObject);
        TreeSpawnSystem.Instance?.OnTreeDestroyed();
    }
}
