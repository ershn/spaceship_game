using System;
using UnityEngine;

public class WorkTask : Task
{
    readonly IWork _work;
    Worker _worker;

    public WorkTask(IWork work)
    {
        _work = work;
    }

    public override void Attach(GameObject executor)
    {
        _worker = executor.GetComponent<Worker>();
    }

    protected override void OnStart(Action<bool> onEnd)
    {
        _worker.WorkOn(_work, onEnd);
    }

    protected override void OnCancel()
    {
        _worker.Cancel();
    }
}
