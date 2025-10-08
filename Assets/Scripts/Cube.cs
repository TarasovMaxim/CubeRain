using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Color _color;
    public event Action ContactWithPlatform;
    public Action ReleaseAction;

    void OnCollisionEnter(Collision collision)
    {
        ContactWithPlatform?.Invoke();
    }
}