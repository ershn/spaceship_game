using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class DeliverItemJob : IJob
{
    readonly ItemAmount _item;
    readonly ulong _amount;
    readonly IInventoryAdd _inventory;

    public DeliverItemJob(ItemAmount item, ulong amount, IInventoryAdd inventory)
    {
        _item = item;
        _amount = amount;
        _inventory = inventory;

        _item.Reserve(_amount);
    }

    public async Task Execute(GameObject executor, CancellationToken ct)
    {
        var pathFinder = executor.GetComponent<PathFinder>();

        try
        {
            ct.ThrowIfCancellationRequested();

            await pathFinder.MoveTo(_item.transform.position, ct);
        }
        catch (TaskCanceledException)
        {
            _item.Unreserve(_amount);
            throw;
        }

        var backpack = executor.GetComponent<Backpack>();
        _item.Remove(_amount);
        backpack.Add(_item.Def, _amount);

        try
        {
            await pathFinder.MoveTo(_inventory.transform.position, ct);

            backpack.Remove(_item.Def, _amount);
            _inventory.Add(_item.Def, _amount);
        }
        catch (TaskCanceledException)
        {
            backpack.Dump();
            throw;
        }
    }
}
