using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemRequestManager : MonoBehaviour
{
    public TaskEvent OnTaskCreation;

    public ItemCreator ItemCreator;
    public ItemGridIndexer ItemGrid;

    Dictionary<ItemRequest, HashSet<ITask>> _requestToTasks = new();

    // TODO: handle canceled tasks
    // TODO: select the closest items
    public void RequestItemDelivery(ItemRequest request)
    {
        var itemDef = request.ItemDef;
        var requestedAmount = request.Amount;
        var inventory = request.Inventory;

        var tasks = new HashSet<ITask>();
        _requestToTasks[request] = tasks;

        var items = ItemGrid
            .GetAllItems(itemDef)
            .Select(item => item.GetComponent<IAmount>());

        foreach (var item in items)
        {
            var availableAmount = item.Get();
            if (availableAmount == 0)
                continue;

            var markedAmount = requestedAmount >= availableAmount
                ? availableAmount : requestedAmount;
            requestedAmount -= markedAmount;

            var task = CreateItemDeliveryTask(itemDef, markedAmount, item, inventory);
            tasks.Add(task);
            task.Then(_ =>
            {
                tasks.Remove(task);
                if (tasks.Count() == 0)
                    _requestToTasks.Remove(request);
            });

            if (requestedAmount == 0)
                break;
        }

        foreach (var task in tasks)
            OnTaskCreation.Invoke(task);
    }

    public void CancelItemDelivery(ItemRequest request)
    {
        if (!_requestToTasks.TryGetValue(request, out var tasks))
            return;
        foreach (var task in tasks.ToArray())
            task.Cancel();
    }

    ITask CreateItemDeliveryTask(
        ItemDef itemDef, ulong amount, IAmountRemove item, IItemAmountAdd inventory
        )
    {
        Debug.Log($"Request item delivery: {itemDef}, {amount}");

        new TaskNode(new MoveTask(item.transform.position), out var startNode)
        .To(new(new ItemToInventoryTask(item, itemDef, amount)))
        .To(new(new MoveTask(inventory.transform.position)),
            new(new DumpInventoryTask(ItemCreator), out var failureNode))
        .To(new(new InventoryToInventoryTask(inventory, itemDef, amount)),
            failureNode);

        return new GraphTask(startNode);
    }
}