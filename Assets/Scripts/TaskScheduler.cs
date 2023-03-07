using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskScheduler : MonoBehaviour
{
    HashSet<TaskExecutor> _idleExecutors = new();
    HashSet<TaskExecutor> _workingExecutors = new();
    Queue<Tuple<ITask, Action<bool>>> _tasks = new();

    void OnEnable()
    {
        TaskExecutor.OnCreation += AddExecutor;
        TaskExecutor.OnDestruction += RemoveExecutor;
    }

    void OnDisable()
    {
        TaskExecutor.OnCreation -= AddExecutor;
        TaskExecutor.OnDestruction -= RemoveExecutor;
    }

    public void AddExecutor(TaskExecutor executor)
    {
        _idleExecutors.Add(executor);
    }

    public void RemoveExecutor(TaskExecutor executor)
    {
        _idleExecutors.Remove(executor);
    }

    public void QueueTask(ITask task, Action<bool> onEnd)
    {
        Debug.Log($"QueueTask: {task}");
        _tasks.Enqueue(new(task, onEnd));
    }

    void Update()
    {
        AssignTasks();
    }

    void AssignTasks()
    {
        while (_tasks.TryPeek(out var task))
        {
            if (AssignTask(task.Item1, task.Item2))
                _tasks.Dequeue();
            else
                break;
        }
    }

    bool AssignTask(ITask task, Action<bool> onEnd)
    {
        var executor = _idleExecutors.FirstOrDefault();
        if (executor == null)
            return false;

        _idleExecutors.Remove(executor);
        _workingExecutors.Add(executor);

        executor.ExecuteTask(task, success =>
        {
            _workingExecutors.Remove(executor);
            _idleExecutors.Add(executor);
            onEnd(success);
        });

        return true;
    }
}