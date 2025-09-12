using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour, IDamage
{
    [Header("<color=#374A5D>AI</color>")]
    [SerializeField] private Transform[] _patrolNodes;
    [SerializeField] private float _attackDistance = 3.0f;
    [SerializeField] private float _chaseDistance = 8.5f;
    [SerializeField] private float _partolSpeedMult = 1.0f;
    [SerializeField] private float _chaseSpeedMult = 1.75f;

    [Header("<color=#374A5D>Animator</color>")]
    [SerializeField] private string _attackBoolName = "isFiring";
    [SerializeField] private string _chaseBoolName = "isChasing";
    [SerializeField] private string _damageTriggerName = "onDamage";
    [SerializeField] private string _deathTriggerName = "onDeath";
    [SerializeField] private string _patrolBoolName = "isPatrolling";

    [Header("<color=#374A5D>Behaviours</color>")]
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private int _maxHP = 20;

    private int _actualHP = 0;
    private float _patrolSpeed = 0.0f, _chaseSpeed = 0.0f;

    private Animator _animator;
    private Collider _collider;
    private NavMeshAgent _agent;
    private Rigidbody _rb;
    private Transform _actualNode;

    private void Awake()
    {
        _actualHP = _maxHP;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed *= _partolSpeedMult;
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.isKinematic = false;

        _patrolSpeed = _agent.speed * _partolSpeedMult;
        _chaseSpeed = _agent.speed * _chaseSpeedMult;
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();

        _actualNode = SelectPatrolNode();

        _agent.SetDestination(_actualNode.position);

        _animator.SetBool(_patrolBoolName, true);
    }

    private void Update()
    {
        if((_player.transform.position - transform.position).sqrMagnitude <= _chaseDistance * _chaseDistance)
        {
            if(_agent.speed != _chaseSpeed || _agent.isStopped)
            {
                _agent.isStopped = false;
                _agent.speed = _chaseSpeed;
                _animator.SetBool(_chaseBoolName, true);
                _animator.SetBool(_patrolBoolName, false);
            }

            _agent.SetDestination(_player.transform.position);

            if((_player.transform.position - transform.position).sqrMagnitude <= _attackDistance * _attackDistance)
            {
                if (!_agent.isStopped)
                {
                    _agent.isStopped = true;
                    _animator.SetBool(_attackBoolName, true);
                    _animator.SetBool(_chaseBoolName, false);                    
                }

                transform.LookAt(_player.transform);
            }
        }
        else
        {
            if (_agent.speed != _patrolSpeed || _agent.isStopped)
            {
                _agent.SetDestination(_actualNode.position);
                _agent.isStopped = false;
                _agent.speed = _patrolSpeed;
                _animator.SetBool(_patrolBoolName, true);
                _animator.SetBool(_chaseBoolName, false);
            }

            if((_actualNode.position - transform.position).sqrMagnitude <= 0.5f * 0.5f)
            {
                _actualNode = SelectPatrolNode(_actualNode);

                _agent.SetDestination(_actualNode.position);
            }
        }
    }

    private Transform SelectPatrolNode(Transform prevNode = null)
    {
        if (!prevNode)
        {
            return _patrolNodes[Random.Range(0, _patrolNodes.Length)];
        }
        else
        {
            Transform newNode = _patrolNodes[Random.Range(0, _patrolNodes.Length)];

            while (prevNode == newNode)
            {
                newNode = _patrolNodes[Random.Range(0, _patrolNodes.Length)];
            }

            return newNode;
        }
    }

    public void TakeDamage(int dmg)
    {
        _actualHP -= dmg;

        if(_actualHP <= 0)
        {
            _rb.isKinematic = true;
            _collider.enabled = false;

            _animator.SetTrigger(_deathTriggerName);
        }
        else
        {
            _animator.SetTrigger(_damageTriggerName);
        }
    }
}
