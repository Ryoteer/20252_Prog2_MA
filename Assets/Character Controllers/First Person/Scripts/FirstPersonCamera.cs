using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("<color=#6699CC>Camera</color>")]
    [Range(-90.0f, 0.0f)][SerializeField] private float _minRotation = -45.0f;
    [Range(0.0f, 90.0f)][SerializeField] private float _maxRotation = 75.0f;

    private float _mouseY = 0.0f;

    private Transform _head;
    public Transform Head
    {
        get { return _head; }
        set { _head = value; }
    }

    private void LateUpdate()
    {
        transform.position = _head.position;
    }

    public void Rotate(float x, float y)
    {
        _mouseY += y;
        _mouseY = Mathf.Clamp(_mouseY, _minRotation, _maxRotation);

        transform.rotation = Quaternion.Euler(-_mouseY, x, 0.0f);
    }
}
