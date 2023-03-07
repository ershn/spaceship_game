using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskExecutor : MonoBehaviour
{
    public static event Action<TaskExecutor> OnCreation;
    public static event Action<TaskExecutor> OnDestruction;

    ITask _task;

    void Start()
    {
        OnCreation?.Invoke(this);
    }

    void OnDestroy()
    {
        OnDestruction?.Invoke(this);
    }

    public void ExecuteTask(ITask task, Action<bool> onEnd)
    {
        if (IsTaskExecuting())
            throw new ArgumentException("A task is already executing.");

        Debug.Log($"ExecuteTask: {task}");
        StartTask(task, onEnd);
    }

    bool IsTaskExecuting() => _task != null;

    void StartTask(ITask task, Action<bool> onEnd)
    {
        _task = task;
        _task.Setup(gameObject);
        _task.Start(success => CompleteTask(success, onEnd));
    }

    void CompleteTask(bool success, Action<bool> onEnd)
    {
        _task = null;
        onEnd(success);
    }
}