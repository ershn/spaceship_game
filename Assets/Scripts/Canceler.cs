using UnityEngine;
using UnityEngine.Events;

public class Canceler : MonoBehaviour
{
    public UnityEvent OnCancel;

    public void Cancel()
    {
        OnCancel.Invoke();
    }
}
