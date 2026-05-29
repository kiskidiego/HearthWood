using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    static Action _alertEnemies;
    public static void AlertAllEnemies()
    {
        _alertEnemies?.Invoke();
    }
    [SerializeField] float _startChaseChancePerSecond = 0.01f;
    [SerializeField] float _chaseChanceIncreasePerSecond = 0.001f;
    [SerializeField] float _stopChaseChancePerSecond = 0.05f;
    [SerializeField] float _stoppingChaseChanceDecreasePerSecond = 0.0001f;
    [SerializeField] float _wanderChangeDirectionChancePerSecond = 0.1f;
    [SerializeField] float _chaseDistance = 50f;
    [SerializeField] Vector4 _traverseableArea = new Vector4(0, 0, 1000, 1000); // xMin, yMin, xMax, yMax
    [SerializeField] float _maxHeight = 600f;
    [SerializeField] float _minHeight = 100f;
    [SerializeField] float _chaseSpeed = 10f;
    [SerializeField] float _normalSpeed = 3.5f;
    [SerializeField] float _attackRange = 1f;
    [SerializeField] Transform _eyes;
    [SerializeField] LayerMask _playerLayerMask;
    [SerializeField] float _attackCooldown = 2f;

    Transform _playerTransform;
    NavMeshAgent _navMeshAgent;
    bool _isChasing = false;
    float _startChaseChancePerSecondCurrent;
    float _stopChaseChancePerSecondCurrent;
    Vector3 _currentWanderTarget;
    bool _isFleeing = false;
    Animator _animator;
    bool _canAttack = true;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        _stopChaseChancePerSecondCurrent = _stopChaseChancePerSecond;
        StopChasing();
    }

    void OnEnable()
    {
        _alertEnemies += StartChasing;
    }
    void OnDisable()
    {
        _alertEnemies -= StartChasing;
    }

    void Update()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude / _chaseSpeed);
        if (_playerTransform == null) return;

        if(_isFleeing)
        {
            if (Vector3.Distance(transform.position, _navMeshAgent.destination) < 1f)
            {
                StopFleeing();
            }
            return;
        }
        else if(!_isChasing)
        {
            CheckChasing();
        }
        else
        {
            CheckStoppingChase();
        }

        if (_isChasing)
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
            CheckAttack();
        }
        else if (Random.value < _wanderChangeDirectionChancePerSecond * Time.deltaTime || Vector3.Distance(transform.position, _currentWanderTarget) < 1f)
        {
            Wander();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (_isFleeing) return;
        if (other.CompareTag("Fire"))
        {
            _navMeshAgent.velocity = Vector3.zero; // Stop movement immediately
            _isChasing = false;
            ResetChaseChances();
            Vector3 fleeDirection = (transform.position - other.transform.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDirection * _chaseDistance * 1.5f;
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(fleeTarget, out navMeshHit, _chaseDistance, NavMesh.AllAreas))
            {
                _navMeshAgent.SetDestination(navMeshHit.position);
                StartFleeing();
            }
        }
    }

    void Wander()
    {
        RaycastHit hit;
        Vector3 randomPoint = new Vector3(
            Random.Range(_traverseableArea.x, _traverseableArea.z),
            _maxHeight,
            Random.Range(_traverseableArea.y, _traverseableArea.w)
        );
        if (Physics.Raycast(randomPoint, Vector3.down, out hit, _maxHeight - _minHeight, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(hit.point, out navMeshHit, 100f, NavMesh.AllAreas))
            {
                _currentWanderTarget = navMeshHit.position;
                _navMeshAgent.SetDestination(_currentWanderTarget);
                return;
            }
        }
    }

    void CheckChasing()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer <= _chaseDistance)
        {
            StartChasing();
            return;
        }
        if (!_isChasing)
        {
            if (Random.value < _startChaseChancePerSecondCurrent * Time.deltaTime)
            {
                StartChasing();
            }
            else
            {
                _startChaseChancePerSecondCurrent += _chaseChanceIncreasePerSecond * Time.deltaTime;
            }
        }
    }

    void CheckStoppingChase()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer < _chaseDistance)
        {
            return;
        }
        if (Random.value < _stopChaseChancePerSecondCurrent * Time.deltaTime)
        {
            StopChasing();
        }
        else
        {
            _stopChaseChancePerSecondCurrent = _stoppingChaseChanceDecreasePerSecond * Time.deltaTime;
        }
    }

    void ResetChaseChances()
    {
        _startChaseChancePerSecondCurrent = _startChaseChancePerSecond;
    }

    void StartChasing()
    {
        _isFleeing = false;
        _isChasing = true;
        _navMeshAgent.speed = _chaseSpeed;
        ResetChaseChances();
    }

    void StopChasing()
    {
        _isChasing = false;
        _navMeshAgent.speed = _normalSpeed;
        ResetChaseChances();
        Wander();
    }

    void StartFleeing()
    {
        _isFleeing = true;
        _navMeshAgent.speed = _chaseSpeed;
    }

    void StopFleeing()
    {
        _isFleeing = false;
        _navMeshAgent.speed = _normalSpeed;
        Wander();
    }

    void StartAttack()
    {
        _canAttack = false;
        _animator.SetTrigger("Attack");
        _navMeshAgent.isStopped = true;
    }

    public void OnAttackAnimationEnd()
    {
        _navMeshAgent.isStopped = false;
        StartCoroutine(AttackCooldown());
    }

    void CheckAttack()
    {
        if (!_canAttack) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer < _attackRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(_eyes.position, _eyes.forward, out hit, _attackRange, _playerLayerMask, QueryTriggerInteraction.Ignore)
                || Physics.Raycast(transform.position, transform.forward, out hit, _attackRange, _playerLayerMask, QueryTriggerInteraction.Ignore)
            )
            {
                StartAttack();
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }
}
