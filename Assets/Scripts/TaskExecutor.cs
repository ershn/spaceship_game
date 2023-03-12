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

    public void ExecuteTask(ITask task)
    {
        if (IsTaskExecuting())
            throw new ArgumentException("A task is already executing.");

        Debug.Log($"ExecuteTask: {task}");
        StartTask(task);
    }

    bool IsTaskExecuting() => _task != null;

    void StartTask(ITask task)
    {
        _task = task;
        _task.Attach(gameObject);
        _task.Then(_ => _task = null);
        _task.Start();
    }
}