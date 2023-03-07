using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float Speed = 3f;

    Rigidbody2D _rigidbody;

    Vector2 _direction;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void UpdateDirection(Vector2 direction)
    {
        _direction = direction;
    }

    void FixedUpdate()
    {
        _rigidbody.position += Speed * Time.deltaTime * _direction;
    }
}
