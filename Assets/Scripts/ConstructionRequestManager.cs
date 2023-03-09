using System;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionRequestManager : MonoBehaviour
{
    public static ConstructionRequestManager Instance { get; private set; }

    public TaskEvent OnTaskCreation;

    Dictionary<IWork, ITask> _requestToTask = new();

    void Awake()
    {
        if (Instance != null)
            throw new InvalidOperationException("A singleton instance already exists.");

        Instance = this;
    }

    // TODO: handle canceled tasks
    public void RequestConstruction(IWork work)
    {
        var task = new TaskSequence(new ITask[]
        {
            new MoveTask(work.transform.position),
            new WorkTask(work)
        });

        _requestToTask[work] = task;
        task.Then(_ => _requestToTask.Remove(work));

        OnTaskCreation.Invoke(task);
    }

    public void CancelConstruction(IWork work)
    {
        if (_requestToTask.TryGetValue(work, out var task))
            task.Cancel();
    }
}