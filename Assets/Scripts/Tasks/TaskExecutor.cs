using UnityEngine;
using UnityEngine.Assertions;

public class TaskExecutor : MonoBehaviour
{
    TaskScheduler _taskScheduler;

    Task _task;

    void Awake()
    {
        _taskScheduler = GetComponentInParent<WorldInternalIO>().TaskScheduler;

        GetComponent<Death>().OnDeath.AddListener(OnDeath);
    }

    void OnEnable() => _taskScheduler.AddExecutor(this);

    void OnDisable() => _taskScheduler.RemoveExecutor(this);

    void OnDeath()
    {
        enabled = false;
        CancelExecution();
    }

    public void Execute(Task task)
    {
        Assert.IsNull(_task);

        _task = task;
        _task.Attach(gameObject);
        _task.Then(_ => _task = null);
        _task.Start();
    }

    void CancelExecution()
    {
        _task?.Cancel();
    }
}
