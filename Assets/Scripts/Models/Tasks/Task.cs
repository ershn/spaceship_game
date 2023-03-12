using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Task characteristics:
/// <list type="bullet">
/// <item><description>Can Start once</description></item>
/// <item><description>Can Cancel several times</description></item>
/// <item><description>Can Rollback several times</description></item>
/// <item><description>Can Cancel or Start then Cancel</description></item>
/// <item><description>Callbacks execute once</description></item>
/// <item><description>Callbacks will be triggered by Start and Cancel</description></item>
/// <item><description>Callbacks execute in reverse order</description></item>
/// </list>
/// </summary>

public abstract class Task : ITask
{
    /// <summary>
    /// Set to true on OnStart start.
    /// </summary>
    public bool Started { get; protected set; } = false;

    /// <summary>
    /// Set to true on OnStart end.
    /// </summary>
    public bool Executed { get; protected set; } = false;

    /// <summary>
    /// Set to true on effective Cancel.
    /// </summary>
    public bool Canceled { get; protected set; } = false;

    /// <summary>
    /// Set to the result of Start/Cancel.
    /// </summary>
    public bool? Succeeded { get; protected set; }

    /// <summary>
    /// Set to true on effective Rollback.
    /// </summary>
    public bool RolledBack { get; protected set; } = false;

    public abstract void Attach(GameObject executor);

    public void Start()
    {
        if (Canceled || Started)
            throw new InvalidOperationException("Task already started/canceled");

        Started = true;
        OnStart(success =>
        {
            Executed = true;
            OnEnd(success);
        });
    }

    /// <summary>
    /// Only called on first Start if not already Canceled.
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
    /// Only called on first Cancel if already Started and not already Executed.
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
        Succeeded = success;

        if (!success)
            OnFailure(Executed);

        foreach (var callback in _callbacks)
            callback(success);
    }

    /// <summary>
    /// Called on task cancelation or failure.
    /// </summary>
    protected virtual void OnFailure(bool executed)
    {
    }

    public void Rollback()
    {
        if (RolledBack)
            return;

        if (Executed && (bool)Succeeded)
        {
            RolledBack = true;
            OnRollback();
        }
    }

    /// <summary>
    /// Only called on first Rollback if Executed and Succeeded.
    /// </summary>
    protected virtual void OnRollback()
    {
    }
}