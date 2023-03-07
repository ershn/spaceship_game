using System;
using System.Linq;
using UnityEngine;

public class CompositeTask : ITask
{
    ITask[] _tasks;
    int _taskIndex;
    Action<bool> _onEnd;

    public CompositeTask(ITask[] tasks)
    {
        _tasks = tasks;
        _taskIndex = 0;
    }

    public void Setup(GameObject executor)
    {
        foreach (var task in _tasks)
        {
            task.Setup(executor);
        }
    }

    public void Start(Action<bool> onEnd)
    {
        _onEnd = onEnd;
        ExecuteNextTask();
    }

    void ExecuteNextTask(bool carryOn = true)
    {
        if (!carryOn)
        {
            _onEnd(false);
            return;
        }

        if (_taskIndex == _tasks.Length)
        {
            _onEnd(true);
            return;
        }

        var task = _tasks[_taskIndex];
        _taskIndex++;
        task.Start(ExecuteNextTask);
    }

    public override string ToString()
    {
        var components = _tasks
            .Select(task => task.ToString())
            .Aggregate((prev, next) => $"{prev}, {next}");
        return $"{base.ToString()}: {components}";
    }
}
