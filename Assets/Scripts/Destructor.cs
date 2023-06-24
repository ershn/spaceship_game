using UnityEngine;
using UnityEngine.Events;

public class Destructor : MonoBehaviour
{
    public UnityEvent OnDestruction;

    public void Destroy()
    {
        OnDestruction.Invoke();
        Destroy(gameObject);
    }
}
