using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemAllotter : MonoBehaviour
{
    class Demand
    {
        public ItemDef ItemDef;
        public ulong Amount;
        public IInventoryAdd Inventory;

        public bool Canceled;
        public HashSet<Task> Tasks = new();

        public void Cancel()
        {
            if (Canceled)
                return;

            Canceled = true;
            foreach (var task in Tasks.ToArray())
                task.Cancel();
        }
    }

    ItemGridIndex _itemGrid;
    TaskScheduler _taskScheduler;

    Dictionary<ItemDef, Queue<Demand>> _itemDemands = new();

    void Awake()
    {
        _itemGrid = transform.root.GetComponent<GridIndexes>().ItemGrid;
        _taskScheduler = transform.root.GetComponent<WorldInternalIO>().TaskScheduler;

        GetComponent<ItemCreator>().OnItemCreated += Allot;
    }

    public Action Request(ItemDef itemDef, ulong amount, IInventoryAdd inventory)
    {
        var demand = new Demand()
        {
            ItemDef = itemDef,
            Amount = amount,
            Inventory = inventory
        };

        DeliverWorldItems(demand);
        if (demand.Amount > 0)
            QueueDemand(demand);

        return demand.Cancel;
    }

    void DeliverWorldItems(Demand demand)
    {
        var items = _itemGrid.Filter(demand.ItemDef).CumulateAmount(demand.Amount);

        foreach (var (itemAmount, markedAmount) in items)
            DeliverItem(demand, itemAmount, markedAmount);
    }

    void DeliverItem(Demand demand, ItemAmount itemAmount, ulong amount)
    {
        var task = TaskCreator.DeliverItem(itemAmount, amount, demand.Inventory);

        demand.Amount -= amount;
        demand.Tasks.Add(task);
        task.Then(_ => demand.Tasks.Remove(task));

        _taskScheduler.QueueTask(task);
    }

    void QueueDemand(Demand demand)
    {
        if (!_itemDemands.TryGetValue(demand.ItemDef, out var demands))
        {
            demands = new();
            _itemDemands[demand.ItemDef] = demands;
        }
        demands.Enqueue(demand);
    }

    void Allot(ItemAmount itemAmount)
    {
        if (!_itemDemands.TryGetValue(itemAmount.Def, out var demands))
            return;

        while (demands.TryPeek(out var demand))
        {
            if (demand.Canceled)
            {
                demands.Dequeue();
                continue;
            }

            var markedAmount = Math.Min(itemAmount.Amount, demand.Amount);
            DeliverItem(demand, itemAmount, markedAmount);

            if (demand.Amount == 0)
                demands.Dequeue();

            if (itemAmount.Amount == 0)
                break;
        }
    }
}
