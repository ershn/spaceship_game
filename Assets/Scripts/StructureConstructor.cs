using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class StructureConstructor : MonoBehaviour
{
    public UnityEvent OnConstructionCompleted;
    public UnityEvent OnConstructionCanceled;

    public async void Construct()
    {
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var unregister = GetComponent<Canceler>().OnCancel.Register(cts.Cancel);
        try
        {
            await RequestComponents(ct);
            await RequestConstruction(ct);
            OnConstructionCompleted.Invoke();
        }
        catch (TaskCanceledException)
        {
            GetComponent<Destructor>().Destroy();
            OnConstructionCanceled.Invoke();
            // throw; //? Should the exception be re-thrown ?
        }
        finally
        {
            unregister();
        }
    }

    async Task RequestComponents(CancellationToken ct)
    {
        var componentInventory = GetComponent<StructureComponentInventory>();
        if (componentInventory.Full)
            return;

        var itemAllotter = GetComponentInParent<WorldInternalIO>().ItemAllotter;
        var requests = new List<Task>();
        foreach (var (itemDef, missingAmount) in componentInventory.UnfilledSlots())
        {
            var request = itemAllotter.Request(itemDef, missingAmount, componentInventory, ct);
            requests.Add(request);
        }

        await Task.WhenAll(requests);
    }

    async Task RequestConstruction(CancellationToken ct)
    {
        var jobScheduler = GetComponentInParent<WorldInternalIO>().JobScheduler;
        var constructionWork = GetComponent<ConstructionWork>();

        var job = new WorkOnJob(constructionWork);
        await jobScheduler.Execute(job, ct);

        Destroy(this);
        Destroy(constructionWork);
    }
}
