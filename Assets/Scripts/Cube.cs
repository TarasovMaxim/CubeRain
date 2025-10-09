using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour
{
    public event Action<Cube> CubeReleasingToPool;
    private ColorChanger _colorChanger;
    private Color _startColor;
    private bool _isCollisionEnter = false;

    private void Awake()
    {
        _colorChanger = this.GetComponent<ColorChanger>();
        _startColor = _colorChanger.GetStartColor();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.TryGetComponent<Platform>(out var platform) && _isCollisionEnter == false)
        {
            _colorChanger.ChangeColor(this);
            StartCoroutine(DelayedRelease());
            _isCollisionEnter = true;
        }
    }

    private IEnumerator DelayedRelease()
    {
        float minDelay = 2f;
        float maxDelay = 5f;
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        CubeReleasingToPool?.Invoke(this);
        _isCollisionEnter = false;
        _colorChanger.ChangeColor(this, _startColor);
    }
}