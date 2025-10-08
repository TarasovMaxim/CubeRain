using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Color _color;
    public event Action <Cube> ContactWithPlatform;
    //public Action ReleaseAction;

    void OnCollisionEnter(Collision collision)
    {
        ContactWithPlatform?.Invoke(this);
    }
}