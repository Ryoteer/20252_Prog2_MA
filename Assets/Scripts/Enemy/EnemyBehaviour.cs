using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBehaviour : MonoBehaviour, IDamage
{
    [Header("<color=#374A5D>Animator</color>")]
    [SerializeField] private string _damageTriggerName = "onDamage";
    [SerializeField] private string _deathTriggerName = "onDeath";

    [Header("<color=#374A5D>Behaviours</color>")]
    [SerializeField] private int _maxHP = 20;

    private int _actualHP = 0;

    private Animator _animator;
    private Collider _collider;
    private Rigidbody _rb;

    private void Awake()
    {
        _actualHP = _maxHP;
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.isKinematic = false;
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
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
