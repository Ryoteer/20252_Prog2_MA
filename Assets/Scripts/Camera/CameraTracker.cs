using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _targetTransform;

    private Vector3 _offset = new();

    private void Start()
    {
        _offset = transform.position - _targetTransform.position;
    }

    private void LateUpdate()
    {
        transform.position = _targetTransform.position + _offset;
    }
}
