using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class TransactionalWork : MonoBehaviour, IWork
{
    enum Phase
    {
        Pending,
        Started,
        Completed
    }

    public UnityEvent OnWorkStarted;
    public FloatEvent OnWorkProgress;
    public UnityEvent OnWorkCompleted;
    public UnityEvent OnWorkReset;

    protected abstract float RequiredTime { get; }

    Phase _phase = Phase.Pending;
    float _currentTime = 0f;

    public bool Work(float time)
    {
        if (_phase == Phase.Completed)
            throw new InvalidOperationException("Work already completed");

        if (_phase == Phase.Pending)
        {
            _phase = Phase.Started;
            OnWorkStarted.Invoke();
        }

        _currentTime += time;

        var progress = Mathf.Clamp01(_currentTime / RequiredTime);
        OnWorkProgress.Invoke(progress);

        if (progress >= 1f)
        {
            _phase = Phase.Completed;
            OnWorkCompleted.Invoke();
        }

        return _phase == Phase.Completed;
    }

    public void Reset()
    {
        _phase = Phase.Pending;
        _currentTime = 0f;
        OnWorkReset.Invoke();
    }
}