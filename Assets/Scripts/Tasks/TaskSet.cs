using System.Collections.Generic;

public class TaskSet : ITaskSet
{
    readonly HashSet<ITask> _tasks;

    public TaskSet(IEnumerable<ITask> tasks)
    {
        _tasks = new();
        foreach (var task in tasks)
        {
            _tasks.Add(task);
            task.Then(_ => _tasks.Remove(task));
        }
    }

    public void Cancel()
    {
        foreach (var task in _tasks)
            task.Cancel();
    }

    public IEnumerable<ITask> AsEnumerable() => _tasks;
}