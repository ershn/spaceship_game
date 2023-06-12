using System;
using UnityEngine.Events;

public static class UnityEventExtensions
{
    public static Action Register(this UnityEvent evt, UnityAction action)
    {
        evt.AddListener(action);
        return () => evt.RemoveListener(action);
    }

    public static Action Register<T0>(this UnityEvent<T0> evt, UnityAction<T0> action)
    {
        evt.AddListener(action);
        return () => evt.RemoveListener(action);
    }
}
