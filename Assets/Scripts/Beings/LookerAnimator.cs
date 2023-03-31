using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookerAnimator : MonoBehaviour
{
    Animator _animator;

    Vector2 _lookDirection;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateLookDirection(Vector2 lookDirection)
    {
        _lookDirection = lookDirection;
    }

    void FixedUpdate()
    {
        _animator.SetFloat("Move X", _lookDirection.x);
        _animator.SetFloat("Move Y", _lookDirection.y);
    }
}

