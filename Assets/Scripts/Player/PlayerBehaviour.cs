using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    [Header("<color=#00541B>Animation</color>")]
    [SerializeField] private string _attackTriggerName = "onAttack";
    [SerializeField] private string _fireTriggerName = "onFire";
    [SerializeField] private string _groundBoolName = "isGrounded";
    [SerializeField] private string _jumpTriggerName = "onJump";
    [SerializeField] private string _moveBoolName = "isMoving";
    [SerializeField] private string _throwTriggerName = "onThrow";
    [SerializeField] private string _xFloatName = "xAxis";
    [SerializeField] private string _zFloatName = "zAxis";

    [Header("<color=#00541B>Combat</color>")]
    [SerializeField] private float _attackDistance = 1.0f;
    [SerializeField] private float _attackRadius = 0.25f;
    [SerializeField] private int _baseDmg = 5;
    [SerializeField] private LayerMask _entityMask;
    [SerializeField] private float _fireDistance = 25.0f;
    [SerializeField] private int _fireDmgMult = 2;
    [SerializeField] private Transform _fireOrigin;
    [SerializeField] private float _grenadeDistance = 10.0f;
    [SerializeField] private float _grenadeRadius = 2.0f;
    [SerializeField] private int _grenadeDmgMult = 5;

    [Header("<color=#00541B>Inputs</color>")]
    [SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _firekKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _throwKey = KeyCode.G;

    [Header("<color=#00541B>Physics</color>")]
    [SerializeField] private float _groundDistance = 0.25f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _jumpForce = 7.5f;
    [SerializeField] private float _moveDistance = 0.625f;
    [SerializeField] private LayerMask _moveMask;
    [SerializeField] private float _moveSpeed = 3.5f;

    private Animator _animator;
    private Collider[] _grenadeHits;
    private PlayerAvatar _avatar;
    private Rigidbody _rb;

    private bool _isGrounded = true;

    private Ray _attackRay, _groundRay, _moveRay;
    private RaycastHit[] _combatHits;
    private RaycastHit _combatHit;

    private Vector2 _moveInputs = new();
    private Vector3 _attackOffset = new(), _moveDir = new(), _moveRayDir = new(), _transformOffset = new();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        //_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _avatar = GetComponentInChildren<PlayerAvatar>();
    }

    private void Update()
    {
        _transformOffset = transform.position;
        _transformOffset.y += 0.1f;

        _isGrounded = IsGrounded();

        _animator.SetLayerWeight(1, _avatar.legsLayerWeight);

        _moveInputs.x = Input.GetAxis("Horizontal");
        _animator.SetFloat(_xFloatName, _moveInputs.x);
        _moveInputs.y = Input.GetAxis("Vertical");
        _animator.SetFloat(_zFloatName, _moveInputs.y);

        _animator.SetBool(_groundBoolName, _isGrounded);
        _animator.SetBool(_moveBoolName, _moveInputs.sqrMagnitude != 0.0f);

        if (Input.GetKeyDown(_jumpKey))
        {
            _animator.SetTrigger(_jumpTriggerName);
        }

        if (Input.GetKeyDown(_attackKey))
        {
            _animator.SetTrigger(_attackTriggerName);
        }
        else if (Input.GetKeyDown(_firekKey))
        {
            _animator.SetTrigger(_fireTriggerName);
        }
        else if (Input.GetKeyDown(_throwKey))
        {
            _animator.SetTrigger(_throwTriggerName);
        }
    }

    private void FixedUpdate()
    {
        if(_moveInputs.sqrMagnitude != 0.0f && !IsBlocked(_moveInputs))
        {
            Movement(_moveInputs);
        }
    }

    public void Attack()
    {
        _attackOffset = transform.position;
        _attackOffset.y += 1.5f;

        _attackRay = new Ray(_attackOffset, transform.forward);

        if(Physics.SphereCast(_attackRay, _attackRadius, out _combatHit, _attackDistance, _entityMask))
        {
            if(_combatHit.collider.TryGetComponent(out IDamage damage))
            {
                Debug.Log($"<color=#00541B>Big Smoke</color>: YOU PICKED THE WRONG HOUSE, FOOL!");

                damage.TakeDamage(_baseDmg);
            }
        }
    }

    public void Fire()
    {
        _attackRay = new Ray(_fireOrigin.position, transform.forward);

        _combatHits = Physics.RaycastAll(_attackRay, _fireDistance, _entityMask);

        foreach(RaycastHit hit in _combatHits)
        {
            if(hit.collider.TryGetComponent(out IDamage damage))
            {
                damage.TakeDamage(_baseDmg * _fireDmgMult);
            }
        }
    }

    public void Grenade()
    {
        _attackOffset = transform.position;
        _attackOffset.z += _grenadeDistance;

        _grenadeHits = Physics.OverlapSphere(_attackOffset, _grenadeRadius, _entityMask);

        foreach(Collider hit in _grenadeHits)
        {
            if(hit.TryGetComponent(out IDamage damage))
            {
                damage.TakeDamage(_baseDmg * _grenadeDmgMult);
            }
        }
    }

    private bool IsBlocked(Vector2 input)
    {
        _moveRayDir = (transform.right * input.x + transform.forward * input.y);

        _moveRay = new Ray(_transformOffset, _moveRayDir);

        return Physics.Raycast(_moveRay, _moveDistance, _moveMask);
    }

    private bool IsGrounded()
    {
        _groundRay = new Ray(_transformOffset, -transform.up);

        return Physics.Raycast(_groundRay, _groundDistance, _groundMask);
    }

    public void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void Movement(Vector2 input)
    {
        _moveDir = (transform.right * input.x + transform.forward * input.y).normalized;

        _rb.MovePosition(transform.position + _moveDir * _moveSpeed * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        if (_isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawRay(_groundRay.origin, _groundRay.direction * _groundDistance);
    }
}
