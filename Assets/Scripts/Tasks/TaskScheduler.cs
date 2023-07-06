using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskScheduler : MonoBehaviour
{
    struct QueuedTask
    {
        public Task Task;
        public TaskExecutor Executor;
    }

    readonly HashSet<TaskExecutor> _idleExecutors = new();
    readonly HashSet<TaskExecutor> _workingExecutors = new();
    readonly Queue<QueuedTask> _queuedTasks = new();

    public void AddExecutor(TaskExecutor executor)
    {
        _idleExecutors.Add(executor);
    }

    public void RemoveExecutor(TaskExecutor executor)
    {
        _idleExecutors.Remove(executor);
    }

    public void QueueTask(Task task, TaskExecutor executor = null)
    {
        _queuedTasks.Enqueue(new() { Task = task, Executor = executor });
    }

    public void QueueTaskSet(TaskSet taskSet)
    {
        foreach (var task in taskSet.AsEnumerable())
            QueueTask(task);
    }

    void Update()
    {
        AssignTasks();
    }

    void AssignTasks()
    {
        while (_queuedTasks.TryPeek(out var queuedTask))
        {
            if (queuedTask.Task.Canceled || AssignTask(queuedTask))
                _queuedTasks.Dequeue();
            else
                break;
        }
    }

    bool AssignTask(QueuedTask queuedTask)
    {
        var executor = SelectIdleExecutor(queuedTask.Executor);
        if (executor == null)
            return false;

        _idleExecutors.Remove(executor);
        _workingExecutors.Add(executor);

        var task = queuedTask.Task;

        task.Then(_ =>
        {
            _workingExecutors.Remove(executor);
            _idleExecutors.Add(executor);
        });

        executor.Execute(task);

        return true;
    }

    TaskExecutor SelectIdleExecutor(TaskExecutor targetExecutor = null)
    {
        if (targetExecutor != null)
            return _idleExecutors.Contains(targetExecutor) ? targetExecutor : null;
        else
            return _idleExecutors.FirstOrDefault();
    }
}
