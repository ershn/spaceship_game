using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Task characteristics:
/// <list type="bullet">
/// <item><description>Can Start only 1 time</description></item>
/// <item><description>Can Cancel several times</description></item>
/// <item><description>Can Start and Cancel in any order</description></item>
/// <item><description>Callbacks execute only 1 time</description></item>
/// <item><description>Callbacks will be triggered by Start and Cancel</description></item>
/// </list>
/// </summary>

public abstract class Task : ITask
{
    public bool Started  { get; protected set; } = false;
    public bool Executed { get; protected set; } = false;
    public bool Canceled { get; protected set; } = false;

    public abstract void Prepare(GameObject executor);

    public void Start()
    {
        if (Started)
            throw new InvalidOperationException("Task already started");

        Started = true;
        if (Canceled)
            return;

        OnStart(success =>
        {
            Executed = true;
            OnEnd(success);
        });
    }

    /// <summary>
    /// Only called on first start if not already canceled.
    /// </summary>
    protected abstract void OnStart(Action<bool> onEnd);

    public void Cancel()
    {
        if (Canceled || Executed)
            return;

        Canceled = true;
        if (Started)
            OnCancel();
        else
            OnEnd(false);
    }

    /// <summary>
    /// Only called on first cancel if already started and not done.
    /// </summary>
    protected virtual void OnCancel()
    {
    }

    Stack<Action<bool>> _callbacks = new();

    /// <summary>
    /// Added callbacks are executed in reverse order.
    /// </summary>
    public ITask Then(Action<bool> callback)
    {
        _callbacks.Push(callback);
        return this;
    }

    void OnEnd(bool success)
    {
        foreach (var callback in _callbacks)
            callback(success);
    }
}