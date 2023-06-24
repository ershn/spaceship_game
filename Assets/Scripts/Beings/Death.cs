using UnityEngine;
using UnityEngine.Events;

public class Death : MonoBehaviour
{
    public UnityEvent OnDeath;

    public void Die()
    {
        OnDeath.Invoke();
    }
}
