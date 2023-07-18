using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class StructureInventory : MonoBehaviour, IInventoryAdd, IInventoryRemove
{
    class ItemSlot
    {
        public event Action<bool> OnFull;

        public ItemDef ItemDef { get; }

        public ulong MaxAmount { get; }
        public ulong CurrentAmount { get; private set; }

        public ulong? RefillThreshold { get; }
        public ItemAllotter.IRequest RefillRequest { get; set; }

        public ItemSlot(ItemDef itemDef, ulong maxAmount, ulong? refillThreshold = null)
        {
            ItemDef = itemDef;
            MaxAmount = maxAmount;
            RefillThreshold = refillThreshold;
        }

        public void IncreaseAmount(ulong amount)
        {
            Assert.IsTrue(amount > 0);
            Assert.IsTrue(CurrentAmount + amount <= MaxAmount);

            CurrentAmount += amount;
            if (CurrentAmount == MaxAmount)
                OnFull?.Invoke(true);
        }

        public void DecreaseAmount(ulong amount)
        {
            Assert.IsTrue(amount > 0);
            Assert.IsTrue(amount <= CurrentAmount);

            CurrentAmount -= amount;
            if (CurrentAmount + amount == MaxAmount)
                OnFull?.Invoke(false);
        }

        public void ZeroAmount()
        {
            var previousAmount = CurrentAmount;
            CurrentAmount = 0;
            if (previousAmount == MaxAmount)
                OnFull?.Invoke(false);
        }
    }

    public BoolEvent OnFull;

    ItemCreator _itemCreator;
    ItemAllotter _itemAllotter;
    GridPosition _gridPosition;

    readonly Dictionary<ItemDef, ItemSlot> _slots = new();
    uint _fullSlotCount = 0;

    protected virtual void Awake()
    {
        var worldIO = transform.root.GetComponent<WorldInternalIO>();
        _itemCreator = worldIO.ItemCreator;
        _itemAllotter = worldIO.ItemAllotter;
        _gridPosition = GetComponent<GridPosition>();

        GetComponent<Destructor>().OnDestruction.AddListener(Dump);
    }

    public void AddSlot(ItemDef itemDef, ulong maxAmount) => AddSlot(itemDef, maxAmount, null);

    public void AddSlot(ItemDef itemDef, ulong maxAmount, ulong refillThreshold) =>
        AddSlot(itemDef, maxAmount, (ulong?)refillThreshold);

    void AddSlot(ItemDef itemDef, ulong maxAmount, ulong? refillThreshold)
    {
        Assert.IsTrue(maxAmount > 0);
        Assert.IsTrue(refillThreshold == null || refillThreshold < maxAmount);

        var slot = new ItemSlot(itemDef, maxAmount, refillThreshold);
        slot.OnFull += OnSlotFull;
        _slots[itemDef] = slot;

        RequestRefill(slot);
    }

    public bool Full { get; private set; }

    void OnSlotFull(bool full)
    {
        var previouslyFull = Full;

        if (full)
            _fullSlotCount++;
        else
            _fullSlotCount--;

        Full = _fullSlotCount == _slots.Count;
        if (previouslyFull != Full)
            OnFull?.Invoke(Full);
    }

    public IEnumerable<(ItemDef, ulong)> UnfilledSlots() =>
        _slots
            .Where(kv => kv.Value.CurrentAmount < kv.Value.MaxAmount)
            .Select(kv => (kv.Key, kv.Value.MaxAmount - kv.Value.CurrentAmount));

    public void Add(ItemDef itemDef, ulong amount)
    {
        Assert.IsTrue(amount > 0);

        var slot = _slots[itemDef];
        Assert.IsTrue(slot.CurrentAmount + amount <= slot.MaxAmount);

        slot.IncreaseAmount(amount);
    }

    public void Remove(ItemDef itemDef, ulong amount)
    {
        Assert.IsTrue(amount > 0);

        var slot = _slots[itemDef];
        Assert.IsTrue(amount <= slot.CurrentAmount);

        slot.DecreaseAmount(amount);
        RequestRefill(slot);
    }

    public bool TryRemove(ItemDef itemDef, ulong amount)
    {
        Assert.IsTrue(amount > 0);

        var slot = _slots[itemDef];
        if (amount <= slot.CurrentAmount)
        {
            slot.DecreaseAmount(amount);
            RequestRefill(slot);
            return true;
        }
        else
            return false;
    }

    void RequestRefill(ItemSlot slot)
    {
        if (slot.RefillThreshold == null)
            return;

        var undeliveredAmount = slot.RefillRequest?.UndeliveredAmount ?? 0;
        var futureAmount = slot.CurrentAmount + undeliveredAmount;
        if (futureAmount > slot.RefillThreshold)
            return;

        var requestAmount = slot.MaxAmount - futureAmount;
        if (slot.RefillRequest == null)
        {
            slot.RefillRequest = _itemAllotter.Request(slot.ItemDef, requestAmount, this);
            slot.RefillRequest.OnCompleted += _ => slot.RefillRequest = null;
        }
        else
            slot.RefillRequest.IncreaseRequestedAmount(requestAmount);
    }

    public void Dump()
    {
        var cellPosition = _gridPosition.CellPosition;

        foreach (var slot in _slots.Values)
        {
            slot.RefillRequest?.Cancel();

            if (slot.CurrentAmount > 0)
            {
                _itemCreator.Create(cellPosition, slot.ItemDef, slot.CurrentAmount);
                slot.ZeroAmount();
            }
        }
    }
}
