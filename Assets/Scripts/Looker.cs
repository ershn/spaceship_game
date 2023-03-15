using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{
    public Vector2 InitialLookDirection = new(0, -1);

    public Vector2Event OnLookDirectionUpdated;

    Vector2 _direction;

    void Start()
    {
        OnLookDirectionUpdated.Invoke(InitialLookDirection);
    }

    public void UpdateDirection(Vector2 direction)
    {
        _direction = direction;
    }

    void FixedUpdate()
    {
        if (!(Mathf.Approximately(_direction.x, 0.0f)
            && Mathf.Approximately(_direction.y, 0.0f)))
            OnLookDirectionUpdated.Invoke(_direction.normalized);
    }
}
