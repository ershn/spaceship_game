using System.Collections.Generic;
using UnityEngine;

public class ConstructionRequestManager : MonoBehaviour
{
    public TaskEvent OnTaskCreation;

    Dictionary<IWork, ITask> _requestToTask = new();

    // TODO: handle canceled tasks
    public void RequestConstruction(IWork work)
    {
        var task = new SequenceTask(new ITask[]
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