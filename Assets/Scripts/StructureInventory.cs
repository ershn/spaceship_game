using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class StructureInventory : MonoBehaviour, IInventoryAdd, IInventoryRemove
{
    [Serializable]
    class ItemSlot
    {
        public event Action<bool> OnFull;

        [SerializeField]
        ulong _maxAmount;
        public ulong MaxAmount => _maxAmount;

        [SerializeField]
        ulong _currentAmount;
        public ulong CurrentAmount => _currentAmount;

        public ulong? RefillThreshold { get; }
        public ItemAllotter.IRequest RefillRequest { get; set; }

        public ItemSlot(ulong maxAmount, ulong? refillThreshold = null)
        {
            _maxAmount = maxAmount;
            RefillThreshold = refillThreshold;
        }

        public bool Full => _currentAmount == _maxAmount;

        public void IncreaseAmount(ulong amount)
        {
            Assert.IsTrue(amount > 0);
            Assert.IsTrue(_currentAmount + amount <= _maxAmount);

            _currentAmount += amount;
            if (_currentAmount == _maxAmount)
                OnFull?.Invoke(true);
        }

        public void DecreaseAmount(ulong amount)
        {
            Assert.IsTrue(amount > 0);
            Assert.IsTrue(amount <= _currentAmount);

            _currentAmount -= amount;
            if (_currentAmount + amount == _maxAmount)
                OnFull?.Invoke(false);
        }

        public void ZeroAmount()
        {
            var previousAmount = _currentAmount;
            _currentAmount = 0;
            if (previousAmount == _maxAmount)
                OnFull?.Invoke(false);
        }
    }

    public UnityEvent<bool> OnFull;

    ItemCreator _itemCreator;
    ItemAllotter _itemAllotter;
    GridPosition _gridPosition;

    [SerializeField]
    SerializedDictionary<ItemDef, ItemSlot> _slots = new();
    uint _fullSlotCount = 0;

    protected virtual void Awake()
    {
        var worldIO = GetComponentInParent<WorldInternalIO>();
        _itemCreator = worldIO.ItemCreator;
        _itemAllotter = worldIO.ItemAllotter;
        _gridPosition = GetComponent<GridPosition>();

        GetComponent<Destructor>().OnDestruction.AddListener(Dump);

        foreach (var (itemDef, slot) in _slots)
            InitSlot(itemDef, slot);
    }

    public bool Setup => _slots.Count > 0;

    public bool Full => _fullSlotCount == _slots.Count;

    public void AddSlot(ItemDef itemDef, ulong maxAmount) => AddSlot(itemDef, maxAmount, null);

    public void AddSlot(ItemDef itemDef, ulong maxAmount, ulong refillThreshold) =>
        AddSlot(itemDef, maxAmount, (ulong?)refillThreshold);

    void AddSlot(ItemDef itemDef, ulong maxAmount, ulong? refillThreshold)
    {
        Assert.IsTrue(maxAmount > 0);
        Assert.IsTrue(refillThreshold == null || refillThreshold < maxAmount);

        var slot = new ItemSlot(maxAmount, refillThreshold);
        _slots[itemDef] = slot;
        InitSlot(itemDef, slot);
    }

    void InitSlot(ItemDef itemDef, ItemSlot slot)
    {
        slot.OnFull += OnSlotFull;
        RequestRefill(itemDef, slot);
        if (slot.Full)
            _fullSlotCount++;
    }

    void OnSlotFull(bool full)
    {
        var previouslyFull = Full;

        if (full)
            _fullSlotCount++;
        else
            _fullSlotCount--;

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
        RequestRefill(itemDef, slot);
    }

    public bool TryRemove(ItemDef itemDef, ulong amount)
    {
        Assert.IsTrue(amount > 0);

        var slot = _slots[itemDef];
        if (amount <= slot.CurrentAmount)
        {
            slot.DecreaseAmount(amount);
            RequestRefill(itemDef, slot);
            return true;
        }
        else
            return false;
    }

    void RequestRefill(ItemDef itemDef, ItemSlot slot)
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
            slot.RefillRequest = _itemAllotter.Request(itemDef, requestAmount, this);
            slot.RefillRequest.OnCompleted += _ => slot.RefillRequest = null;
        }
        else
            slot.RefillRequest.IncreaseRequestedAmount(requestAmount);
    }

    public void Dump()
    {
        var cellPosition = _gridPosition.CellPosition;

        foreach (var (itemDef, slot) in _slots)
        {
            slot.RefillRequest?.Cancel();

            if (slot.CurrentAmount > 0)
            {
                _itemCreator.Create(cellPosition, itemDef, slot.CurrentAmount);
                slot.ZeroAmount();
            }
        }
    }
}
