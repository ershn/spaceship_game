using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGameEvent<T0> : ScriptableObject
{
    List<GenericGameEventListener<T0>> _listeners = new();

    public void AddListener(GenericGameEventListener<T0> listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(GenericGameEventListener<T0> listener)
    {
        _listeners.Remove(listener);
    }

    public void Invoke(T0 t0)
    {
        _listeners.ForEach(listener => listener.OnEventInvoked(t0));
    }
}

public class GenericGameEvent<T0, T1> : ScriptableObject
{
    List<GenericGameEventListener<T0, T1>> _listeners = new();

    public void AddListener(GenericGameEventListener<T0, T1> listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(GenericGameEventListener<T0, T1> listener)
    {
        _listeners.Remove(listener);
    }

    public void Invoke(T0 t0, T1 t1)
    {
        _listeners.ForEach(listener => listener.OnEventInvoked(t0, t1));
    }
}

public class GenericGameEvent<T0, T1, T2> : ScriptableObject
{
    List<GenericGameEventListener<T0, T1, T2>> _listeners = new();

    public void AddListener(GenericGameEventListener<T0, T1, T2> listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(GenericGameEventListener<T0, T1, T2> listener)
    {
        _listeners.Remove(listener);
    }

    public void Invoke(T0 t0, T1 t1, T2 t2)
    {
        _listeners.ForEach(listener => listener.OnEventInvoked(t0, t1, t2));
    }
}

public class GenericGameEvent<T0, T1, T2, T3> : ScriptableObject
{
    List<GenericGameEventListener<T0, T1, T2, T3>> _listeners = new();

    public void AddListener(GenericGameEventListener<T0, T1, T2, T3> listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(GenericGameEventListener<T0, T1, T2, T3> listener)
    {
        _listeners.Remove(listener);
    }

    public void Invoke(T0 t0, T1 t1, T2 t2, T3 t3)
    {
        _listeners.ForEach(listener => listener.OnEventInvoked(t0, t1, t2, t3));
    }
}
