using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
    [Header("<color=blue>Behaviours</color>")]
    [SerializeField] private float _fadeTime = 3.0f;
    [SerializeField] private float _interval = 5.0f;
    [SerializeField] private float _respawnTime = 3.0f;

    private bool _isActive = false;

    private Collider _collider;
    private Material _material;

    private Color _color;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _material = GetComponentInChildren<Renderer>().material;
        _color = _material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerBehaviour>() && !_isActive)
        {
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        _isActive = true;

        float t = 0.0f;

        while(t < 1.0f)
        {
            t += Time.deltaTime / _fadeTime;

            _material.color = new Color(_color.r, _color.g, _color.b,
                                        Mathf.Lerp(1.0f, 0.0f, t));

            yield return null;
        }

        _collider.enabled = false;

        yield return new WaitForSeconds(_interval);

        t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / _respawnTime;

            _material.color = new Color(_color.r, _color.g, _color.b,
                                        Mathf.Lerp(0.0f, 1.0f, t));

            yield return null;
        }

        _collider.enabled = true;

        _isActive = false;
    }
}
