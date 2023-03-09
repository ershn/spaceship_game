using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemRequestManager : MonoBehaviour
{
    public static ItemRequestManager Instance { get; private set; }

    public TaskEvent OnTaskCreation;

    public ItemGridIndexer ItemGrid;

    Dictionary<ItemRequest, HashSet<ITask>> _requestToTasks = new();

    void Awake()
    {
        if (Instance != null)
            throw new InvalidOperationException("A singleton instance already exists.");

        Instance = this;
    }

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
            item.Reserve(markedAmount);
            requestedAmount -= markedAmount;

            var task = CreateItemDeliveryTask(itemDef, markedAmount, item, inventory);

            tasks.Add(task);
            task.Then(_ =>
            {
                tasks.Remove(task);
                if (tasks.Count() == 0)
                    _requestToTasks.Remove(request);
            });

            OnTaskCreation.Invoke(task);

            if (requestedAmount == 0)
                break;
        }
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
        return new TaskSequence(new ITask[]
        {
            new MoveTask(item.transform.position),
            new ItemToInventoryTask(item, itemDef, amount),
            new MoveTask(inventory.transform.position),
            new InventoryToInventoryTask(inventory, itemDef, amount)
        });
    }
}