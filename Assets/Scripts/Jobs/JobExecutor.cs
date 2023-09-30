using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class JobExecutor : MonoBehaviour
{
    JobScheduler _jobScheduler;

    readonly CancellationTokenSource _taskCanceller = new();
    bool _isExecuting;

    void Awake()
    {
        _jobScheduler = GetComponentInParent<WorldInternalIO>().JobScheduler;

        GetComponent<Death>().OnDeath.AddListener(OnDeath);
    }

    void OnEnable() => _jobScheduler.AddExecutor(this);

    void OnDisable() => _jobScheduler.RemoveExecutor(this);

    void OnDestroy()
    {
        _taskCanceller.Dispose();
    }

    void OnDeath()
    {
        enabled = false;
        _taskCanceller.Cancel();
    }

    public async Task Execute(IJob job, CancellationToken ct)
    {
        Assert.IsFalse(_isExecuting);

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(_taskCanceller.Token, ct);

        _isExecuting = true;
        try
        {
            await job.Execute(gameObject, cts.Token);
        }
        finally
        {
            _isExecuting = false;
        }
    }
}
