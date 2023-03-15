using System;
using System.Linq;
using UnityEngine;

public class SequenceTask : Task
{
    ITask[] _tasks;
    int _nextTaskIndex;
    ITask _task;
    Action<bool> _onEnd;

    public SequenceTask(ITask[] tasks)
    {
        _tasks = tasks;
        _nextTaskIndex = 0;
    }

    public override void Attach(GameObject executor)
    {
        foreach (var task in _tasks)
            task.Attach(executor);
    }

    protected override void OnStart(Action<bool> onEnd)
    {
        _onEnd = onEnd;
        ExecuteNextTask();
    }

    protected override void OnCancel()
    {
        _task.Cancel();
    }

    bool IsDone() => _nextTaskIndex == _tasks.Length;

    void ExecuteNextTask(bool carryOn = true)
    {
        if (!carryOn)
        {
            _onEnd(false);
            return;
        }

        if (IsDone())
        {
            _onEnd(true);
            return;
        }

        _task = _tasks[_nextTaskIndex++];
        _task.Then(ExecuteNextTask);
        _task.Start();
    }

    protected override void OnFailure(bool executed)
    {
        for (var index = _nextTaskIndex; index < _tasks.Length; index++)
            _tasks[index].Cancel();
    }

    public override string ToString()
    {
        var components = _tasks
            .Select(task => task.ToString())
            .Aggregate((prev, next) => $"{prev}, {next}");
        return $"{base.ToString()}: {components}";
    }
}