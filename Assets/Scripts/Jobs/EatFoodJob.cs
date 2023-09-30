using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EatFoodJob : IJob
{
    readonly (ItemAmount itemAmount, ulong markedAmount)[] _foodItems;
    readonly IWork _foodConsumption;

    public EatFoodJob(
        IEnumerable<(ItemAmount itemAmount, ulong markedAmount)> foodItems,
        IWork foodConsumption
    )
    {
        _foodItems = foodItems.ToArray();
        _foodConsumption = foodConsumption;

        foreach (var (itemAmount, markedAmount) in _foodItems)
            itemAmount.Reserve(markedAmount);
    }

    public async Task Execute(GameObject executor, CancellationToken ct)
    {
        var pathFinder = executor.GetComponent<PathFinder>();
        var backpack = executor.GetComponent<Backpack>();

        int nextItemIndex = 0;
        try
        {
            ct.ThrowIfCancellationRequested();

            foreach (var (itemAmount, markedAmount) in _foodItems)
            {
                await pathFinder.MoveTo(itemAmount.transform.position, ct);
                itemAmount.Remove(markedAmount);
                backpack.Add(itemAmount.Def, markedAmount);
                nextItemIndex++;
            }

            var worker = executor.GetComponent<Worker>();
            await worker.WorkOn(_foodConsumption, ct);
        }
        catch (TaskCanceledException)
        {
            for (; nextItemIndex < _foodItems.Length; nextItemIndex++)
            {
                var (itemAmount, markedAmount) = _foodItems[nextItemIndex];
                itemAmount.Unreserve(markedAmount);
            }
            backpack.Dump();

            throw;
        }
    }
}
