using UnityEngine;
using UnityEngine.Events;

public class StructureCanceler : MonoBehaviour
{
    public UnityEvent OnCancel;

    public void Cancel()
    {
        OnCancel.Invoke();
    }
}
