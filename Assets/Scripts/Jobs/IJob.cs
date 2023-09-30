using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IJob
{
    Task Execute(GameObject executor, CancellationToken ct);
}
