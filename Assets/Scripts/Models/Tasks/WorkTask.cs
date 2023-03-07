using System;
using UnityEngine;

public class WorkTask : ITask
{
    IWork _work;
    Worker _worker;

    public WorkTask(IWork work)
    {
        _work = work;
    }

    public void Setup(GameObject executor)
    {
        _worker = executor.GetComponent<Worker>();
    }

    public void Start(Action<bool> onEnd)
    {
        _worker.WorkOn(_work, onEnd);
    }
}
