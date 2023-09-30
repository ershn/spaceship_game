using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class Worker : MonoBehaviour
{
    IWork _work;
    Action<bool> _onEnd;

    public Task WorkOn(IWork work, CancellationToken ct)
    {
        return AsyncUtils.ToAsync(cb => WorkOn(work, cb), Cancel, ct);
    }

    public void WorkOn(IWork work, Action<bool> onEnd)
    {
        Assert.IsNull(_work);

        _work = work;
        _onEnd = onEnd;
    }

    public void Cancel()
    {
        Assert.IsNotNull(_work);

        _work = null;
        _onEnd(false);
    }

    void Update()
    {
        if (_work == null)
            return;

        var finished = _work.Work(Time.deltaTime);
        if (finished)
        {
            _work = null;
            _onEnd(true);
        }
    }
}
