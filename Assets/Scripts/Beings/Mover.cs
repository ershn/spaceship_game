using UnityEngine;

public class Mover : MonoBehaviour
{
    public float Speed = 3f;

    Rigidbody2D _rigidbody2D;

    Vector2 _moveDirection;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void UpdateMoveDirection(Vector2 moveDirection)
    {
        _moveDirection = moveDirection;
    }

    void FixedUpdate()
    {
        _rigidbody2D.position += Speed * Time.deltaTime * _moveDirection;
    }
}
