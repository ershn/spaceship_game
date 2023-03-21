using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskScheduler : MonoBehaviour
{
    HashSet<TaskExecutor> _idleExecutors = new();
    HashSet<TaskExecutor> _workingExecutors = new();
    Queue<ITask> _tasks = new();

    public void AddExecutor(TaskExecutor executor)
    {
        _idleExecutors.Add(executor);
    }

    public void RemoveExecutor(TaskExecutor executor)
    {
        _idleExecutors.Remove(executor);
    }

    public void QueueTask(ITask task)
    {
        Debug.Log($"QueueTask: {task}");
        _tasks.Enqueue(task);
    }

    void Update()
    {
        AssignTasks();
    }

    void AssignTasks()
    {
        while (_tasks.TryPeek(out var task))
        {
            if (task.Canceled || AssignTask(task))
                _tasks.Dequeue();
            else
                break;
        }
    }

    bool AssignTask(ITask task)
    {
        var executor = _idleExecutors.FirstOrDefault();
        if (executor == null)
            return false;

        _idleExecutors.Remove(executor);
        _workingExecutors.Add(executor);

        task.Then(_ =>
        {
            _workingExecutors.Remove(executor);
            _idleExecutors.Add(executor);
        });

        executor.ExecuteTask(task);

        return true;
    }
}