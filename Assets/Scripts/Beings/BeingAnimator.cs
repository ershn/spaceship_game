using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BeingAnimator : MonoBehaviour
{
    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnDeath()
    {
        _animator.SetTrigger("Die");
    }
}
