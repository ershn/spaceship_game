using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class WorkOnJob : IJob
{
    readonly IWork _work;

    public WorkOnJob(IWork work)
    {
        _work = work;
    }

    public async Task Execute(GameObject executor, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var pathFinder = executor.GetComponent<PathFinder>();
        await pathFinder.MoveTo(_work.transform.position, ct);

        var worker = executor.GetComponent<Worker>();
        await worker.WorkOn(_work, ct);
    }
}
