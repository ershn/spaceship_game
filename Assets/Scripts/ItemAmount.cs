using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class ItemAmount : MonoBehaviour, ITemplate<ItemDef>
{
    static readonly ulong s_defaultMass = 100.KiloGrams();
    static readonly ulong s_defaultCount = 1;

    public void Template(ItemDef itemDef)
    {
        _amount = itemDef.AmountMode switch
        {
            MassMode => s_defaultMass,
            CountMode => s_defaultCount,
            _ => throw new NotImplementedException(),
        };
    }

    public UnityEvent OnAmountChangedToZero;
    public UlongEvent OnAmountChanged;

    ItemDef _itemDef;

    [SerializeField, Amount]
    ulong _amount = 1;
    ulong _reservedAmount = 0;

    bool _started;

    void Awake()
    {
        _itemDef = GetComponent<ItemDefHolder>().ItemDef;
    }

    void Start()
    {
        _started = true;
        SendAmountChangedEvent(_amount);
    }

    public ItemDef Def => _itemDef;

    public void Initialize(ulong amount)
    {
        Assert.IsFalse(_started);

        _amount = amount;
    }

    public ulong Amount => _amount - _reservedAmount;

    public void Add(ulong amount)
    {
        _amount += amount;
        SendAmountChangedEvent(_amount);
    }

    public void Reserve(ulong amount)
    {
        Assert.IsTrue(amount <= _amount - _reservedAmount);

        _reservedAmount += amount;
    }

    public void Unreserve(ulong amount)
    {
        Assert.IsTrue(amount <= _reservedAmount);

        _reservedAmount -= amount;
    }

    public void Remove(ulong amount)
    {
        Assert.IsTrue(amount <= _reservedAmount);

        _reservedAmount -= amount;
        _amount -= amount;
        SendAmountChangedEvent(_amount);
    }

    void SendAmountChangedEvent(ulong amount)
    {
        OnAmountChanged.Invoke(amount);
        if (amount == 0)
            OnAmountChangedToZero.Invoke();
    }
}
