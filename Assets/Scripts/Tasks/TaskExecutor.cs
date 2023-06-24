using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Death))]
public class TaskExecutor : MonoBehaviour
{
    public TaskScheduler TaskScheduler;

    ITask _task;

    void Awake()
    {
        GetComponent<Death>().OnDeath.AddListener(OnDeath);
    }

    void OnEnable() => TaskScheduler.AddExecutor(this);

    void OnDisable() => TaskScheduler.RemoveExecutor(this);

    void OnDeath()
    {
        CancelExecution();
        enabled = false;
    }

    public void Execute(ITask task)
    {
        Assert.IsNull(_task);

        _task = task;
        _task.Attach(gameObject);
        _task.Then(_ => _task = null);
        _task.Start();
    }

    void CancelExecution()
    {
        if (_task == null)
            return;

        _task.Cancel();
    }
}
