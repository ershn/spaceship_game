using System;
using UnityEngine;

public class TaskExecutor : MonoBehaviour
{
    public TaskScheduler TaskScheduler;

    ITask _task;

    void Start()
    {
        TaskScheduler.AddExecutor(this);
    }

    void OnDestroy()
    {
        TaskScheduler.RemoveExecutor(this);
    }

    public void ExecuteTask(ITask task)
    {
        if (IsTaskExecuting())
            throw new ArgumentException("A task is already executing");

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