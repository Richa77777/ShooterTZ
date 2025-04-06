using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable
{
    private enum State { Patrolling, Chasing, Attacking, Reloading }

    [Header("General Settings")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private int _maxHealth = 20;
    
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] _patrolPoints;
    private int _currentPatrolIndex = 0;

    [Header("Detection Settings")]
    [SerializeField] private float _detectionRange = 20f;
    [SerializeField] private float _attackRange = 10f;
    [SerializeField] private float _chaseLimit = 25f;

    [Header("Attack Settings")]
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private Vector2 _attackDelayRange = new(0.2f, 1f);
    [SerializeField, Range(0f, 1f)] private float _missChance = 0.2f;
    [SerializeField] private int _magazineSize = 5;
    [SerializeField] private float _reloadTime = 2f;

    [Header("State Control")]
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private float _lostTargetCooldown = 3f;
    
    private Transform _player;
    private PlayerHealth _playerHealth;
    private NavMeshAgent _agent;
    private State _state = State.Patrolling;

    private bool _canAttack = true;
    private bool _isShooting = false;
    private int _currentAmmo;
    private float _lostTargetTime;

    public int CurrentHealth { get; private set; }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _currentAmmo = _magazineSize;
        CurrentHealth = _maxHealth;
        GoToNextPatrolPoint();
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Patrolling: Patrol(); break;
            case State.Chasing: ChasePlayer(); break;
            case State.Attacking: AttackPlayer(); break;
            case State.Reloading: break;
        }
    }

    private void Patrol()
    {
        if (_agent.remainingDistance < 0.5f)
            GoToNextPatrolPoint();

        if (FindPlayerWithVision())
            _state = State.Chasing;
    }

    private void GoToNextPatrolPoint()
    {
        if (_patrolPoints.Length == 0) return;
        _agent.destination = _patrolPoints[_currentPatrolIndex].position;
        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
    }

    private void ChasePlayer()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);

        if (IsObstacleBetween() || distance > _chaseLimit)
        {
            ResetToPatrolling();
            return;
        }

        if (distance < _attackRange)
        {
            _state = State.Attacking;
            _agent.isStopped = true;
            _lostTargetTime = Time.time;
        }
        else
        {
            _agent.isStopped = false;
            _agent.destination = _player.position;
        }
    }

    private void AttackPlayer()
    {
        if (!_canAttack || IsObstacleBetween())
        {
            if (Time.time - _lostTargetTime >= _lostTargetCooldown)
                ResetToPatrolling();
            return;
        }

        if (!_isShooting)
        {
            _isShooting = true;
            StartCoroutine(StartShooting());
        }
    }

    private IEnumerator StartShooting()
    {
        float delay = Random.Range(_attackDelayRange.x, _attackDelayRange.y);
        yield return new WaitForSeconds(delay);

        while (_state == State.Attacking && !IsObstacleBetween() && _currentAmmo > 0)
        {
            bool isMissed = Random.value < _missChance;
            if (isMissed)
            {
                Debug.Log("Enemy missed the shot!");
            }
            else
            {
                _playerHealth.TakeDamage(_damage);
                Debug.Log($"Enemy hits! Deals {_damage} damage.");
            }

            _currentAmmo--;
            yield return new WaitForSeconds(_fireRate);
        }

        _isShooting = false;

        if (_currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        _state = State.Reloading;
        Debug.Log("Enemy is reloading...");
        yield return new WaitForSeconds(_reloadTime);
        _currentAmmo = _magazineSize;
        Debug.Log("Enemy reloaded!");

        if (_player != null && !IsObstacleBetween() && Vector3.Distance(transform.position, _player.position) < _attackRange)
        {
            _state = State.Attacking;
            AttackPlayer();
        }
        else
        {
            ResetToPatrolling();
        }
    }

    private void ResetToPatrolling()
    {
        _state = State.Patrolling;
        _player = null;
        _agent.isStopped = false;
        _isShooting = false;
        GoToNextPatrolPoint();
    }

    private bool FindPlayerWithVision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectionRange, _playerLayer);

        foreach (var collider in hitColliders)
        {
            PlayerHealth player = collider.GetComponent<PlayerHealth>();
            if (player == null) continue;

            _player = player.transform;
            _playerHealth = _player.GetComponent<PlayerHealth>();
            
            _lostTargetTime = Time.time;
            return !IsObstacleBetween();
        }

        _player = null;
        return false;
    }

    private bool IsObstacleBetween()
    {
        if (_player == null) return true;

        Vector3 direction = (_player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _player.position);

        return Physics.Raycast(transform.position, direction, distance, _obstacleLayer);
    }
    
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage, current health: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy has died!");
        _state = State.Patrolling;
        _agent.isStopped = true;

        EventsHandler.Instance.OnEnemyKilled?.Invoke();
        
        Destroy(gameObject);
    }
}