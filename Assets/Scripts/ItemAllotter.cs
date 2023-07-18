using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ItemAllotter : MonoBehaviour
{
    public interface IRequest
    {
        event Action<bool> OnCompleted;

        bool Completed { get; }
        bool Fulfilled { get; }
        bool Canceled { get; }

        ItemDef ItemDef { get; }
        ulong RequestedAmount { get; }
        ulong AllottedAmount { get; }
        ulong UnallottedAmount { get; }
        ulong DeliveredAmount { get; }
        ulong UndeliveredAmount { get; }

        void IncreaseRequestedAmount(ulong amount);
        void DecreaseRequestedAmount(ulong amount);

        void Cancel();
    }

    class ItemRequest : IRequest
    {
        public event Action<bool> OnCompleted;
        public event Action OnAllotmentReverted;

        public ItemDef ItemDef { get; }

        public ulong RequestedAmount { get; private set; }
        public ulong AllottedAmount { get; private set; }
        public ulong UnallottedAmount => RequestedAmount - AllottedAmount;
        public ulong DeliveredAmount { get; private set; }
        public ulong UndeliveredAmount => RequestedAmount - DeliveredAmount;

        public IInventoryAdd Inventory { get; }

        readonly TaskScheduler _taskScheduler;

        readonly HashSet<Task> _tasks = new();

        public ItemRequest(
            ItemDef itemDef,
            ulong amount,
            IInventoryAdd inventory,
            TaskScheduler taskScheduler
        )
        {
            ItemDef = itemDef;
            RequestedAmount = amount;
            Inventory = inventory;
            _taskScheduler = taskScheduler;
        }

        public bool Completed => Fulfilled || Canceled;

        public void IncreaseRequestedAmount(ulong amount)
        {
            Assert.IsFalse(Completed);

            RequestedAmount += amount;
        }

        public void DecreaseRequestedAmount(ulong amount)
        {
            Assert.IsFalse(Completed);
            Assert.IsTrue(amount <= UnallottedAmount);

            RequestedAmount -= amount;
        }

        public bool Fulfilled { get; private set; }

        void Fulfill()
        {
            Fulfilled = true;
            OnCompleted?.Invoke(true);
        }

        public void Allot(ItemAmount item, ulong amount)
        {
            AllottedAmount += amount;
            var task = TaskCreator.DeliverItem(item, amount, Inventory);
            _tasks.Add(task);
            task.Then(success =>
            {
                _tasks.Remove(task);
                if (success)
                {
                    DeliveredAmount += amount;
                    if (DeliveredAmount == RequestedAmount)
                        Fulfill();
                }
                else
                {
                    AllottedAmount -= amount;
                    OnAllotmentReverted?.Invoke();
                }
            });
            _taskScheduler.QueueTask(task);
        }

        public bool Canceled { get; private set; }

        public void Cancel()
        {
            if (Fulfilled || Canceled)
                return;

            Canceled = true;
            OnCompleted?.Invoke(false);

            foreach (var task in _tasks.ToArray())
                task.Cancel();
        }
    }

    ItemGridIndex _itemGrid;
    TaskScheduler _taskScheduler;

    readonly Dictionary<ItemDef, LinkedList<ItemRequest>> _itemRequests = new();

    void Awake()
    {
        _itemGrid = transform.root.GetComponent<GridIndexes>().ItemGrid;
        _taskScheduler = transform.root.GetComponent<WorldInternalIO>().TaskScheduler;

        GetComponent<ItemCreator>().OnItemCreated += AllotNewItem;
    }

    public IRequest Request(ItemDef itemDef, ulong amount, IInventoryAdd inventory)
    {
        var request = new ItemRequest(itemDef, amount, inventory, _taskScheduler);
        Register(request);
        AllotExistingItems(request);
        return request;
    }

    void Register(ItemRequest request)
    {
        if (!_itemRequests.TryGetValue(request.ItemDef, out var requests))
        {
            requests = new();
            _itemRequests[request.ItemDef] = requests;
        }

        requests.AddLast(request);
        request.OnCompleted += _ => requests.Remove(request);

        request.OnAllotmentReverted += () => AllotExistingItems(request);
    }

    void AllotExistingItems(ItemRequest request)
    {
        var items = _itemGrid.Filter(request.ItemDef).CumulateAmount(request.UnallottedAmount);
        foreach (var (item, markedAmount) in items)
            request.Allot(item, markedAmount);
    }

    void AllotNewItem(ItemAmount item)
    {
        if (!_itemRequests.TryGetValue(item.Def, out var requests))
            return;

        foreach (var request in requests.Where(request => request.UnallottedAmount > 0))
        {
            var markedAmount = Math.Min(item.Amount, request.UnallottedAmount);
            request.Allot(item, markedAmount);
            if (item.Amount == 0)
                break;
        }
    }
}
