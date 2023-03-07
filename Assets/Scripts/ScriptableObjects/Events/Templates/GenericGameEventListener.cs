using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericGameEventListener<T0> : MonoBehaviour
{
    [Serializable]
    public class Event : UnityEvent<T0> {}

    public GenericGameEvent<T0> GameEvent;
    public Event OnEvent;

    void OnEnable()
    {
        GameEvent.AddListener(this);
    }

    void OnDisable()
    {
        GameEvent.RemoveListener(this);
    }

    public void OnEventInvoked(T0 t0)
    {
        OnEvent.Invoke(t0);
    }
}

public class GenericGameEventListener<T0, T1> : MonoBehaviour
{
    [Serializable]
    public class Event : UnityEvent<T0, T1> {}

    public GenericGameEvent<T0, T1> GameEvent;
    public Event OnEvent;

    void OnEnable()
    {
        GameEvent.AddListener(this);
    }

    void OnDisable()
    {
        GameEvent.RemoveListener(this);
    }

    public void OnEventInvoked(T0 t0, T1 t1)
    {
        OnEvent.Invoke(t0, t1);
    }
}

public class GenericGameEventListener<T0, T1, T2> : MonoBehaviour
{
    [Serializable]
    public class Event : UnityEvent<T0, T1, T2> {}

    public GenericGameEvent<T0, T1, T2> GameEvent;
    public Event OnEvent;

    void OnEnable()
    {
        GameEvent.AddListener(this);
    }

    void OnDisable()
    {
        GameEvent.RemoveListener(this);
    }

    public void OnEventInvoked(T0 t0, T1 t1, T2 t2)
    {
        OnEvent.Invoke(t0, t1, t2);
    }
}

public class GenericGameEventListener<T0, T1, T2, T3> : MonoBehaviour
{
    [Serializable]
    public class Event : UnityEvent<T0, T1, T2, T3> {}

    public GenericGameEvent<T0, T1, T2, T3> GameEvent;
    public Event OnEvent;

    void OnEnable()
    {
        GameEvent.AddListener(this);
    }

    void OnDisable()
    {
        GameEvent.RemoveListener(this);
    }

    public void OnEventInvoked(T0 t0, T1 t1, T2 t2, T3 t3)
    {
        OnEvent.Invoke(t0, t1, t2, t3);
    }
}
