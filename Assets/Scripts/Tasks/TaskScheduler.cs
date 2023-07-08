using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskScheduler : MonoBehaviour
{
    readonly HashSet<TaskExecutor> _idleExecutors = new();

    readonly Queue<Task> _pendingCommonTasks = new();
    readonly Dictionary<TaskExecutor, Queue<Task>> _executorTasks = new();

    public void AddExecutor(TaskExecutor executor)
    {
        _executorTasks[executor] = new();
        ProcessIdleExecutor(executor);
    }

    public void RemoveExecutor(TaskExecutor executor)
    {
        _executorTasks.Remove(executor);
    }

    public void QueueTask(Task task)
    {
        ProcessPendingTask(task);
    }

    public void QueueTask(Task task, TaskExecutor executor)
    {
        ProcessPendingTask(task, executor);
    }

    void ProcessIdleExecutor(TaskExecutor executor)
    {
        if (TryDequeueTaskForExecutor(executor, out var task))
            StartExecutor(executor, task);
        else
            ParkExecutor(executor);
    }

    bool TryDequeueTaskForExecutor(TaskExecutor executor, out Task task)
    {
        var executorTasks = _executorTasks[executor];
        while (executorTasks.TryDequeue(out var dequeuedTask))
        {
            if (dequeuedTask.Canceled)
                continue;
            task = dequeuedTask;
            return true;
        }
        while (_pendingCommonTasks.TryDequeue(out var dequeuedTask))
        {
            if (dequeuedTask.Canceled)
                continue;
            task = dequeuedTask;
            return true;
        }
        task = null;
        return false;
    }

    void StartExecutor(TaskExecutor executor, Task task)
    {
        task.Then(_ =>
        {
            if (executor.enabled)
                ProcessIdleExecutor(executor);
        });
        executor.Execute(task);
    }

    void ParkExecutor(TaskExecutor executor)
    {
        _idleExecutors.Add(executor);
    }

    void ProcessPendingTask(Task task)
    {
        var executor = _idleExecutors.FirstOrDefault();
        if (executor != null)
        {
            _idleExecutors.Remove(executor);
            StartExecutor(executor, task);
        }
        else
            _pendingCommonTasks.Enqueue(task);
    }

    void ProcessPendingTask(Task task, TaskExecutor executor)
    {
        if (_idleExecutors.Remove(executor))
            StartExecutor(executor, task);
        else
            _executorTasks[executor].Enqueue(task);
    }
}
