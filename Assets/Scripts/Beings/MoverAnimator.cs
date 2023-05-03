using UnityEngine;

public class MoverAnimator : MonoBehaviour
{
    public Vector2 InitialLookDirection = Vector2.down;

    Animator _animator;

    Vector2 _lookDirection;
    Vector2 _moveDirection;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _lookDirection = InitialLookDirection;
    }

    public void UpdateMoveDirection(Vector2 moveDirection)
    {
        _moveDirection = moveDirection;
    }

    void FixedUpdate()
    {
        var moveMagnitude = _moveDirection.magnitude;

        if (!Mathf.Approximately(moveMagnitude, 0f))
            _lookDirection = _moveDirection.normalized;

        _animator.SetFloat("Look X", _lookDirection.x);
        _animator.SetFloat("Look Y", _lookDirection.y);
        _animator.SetFloat("Speed", moveMagnitude);
    }
}

