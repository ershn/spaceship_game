using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(ItemDefHolder))]
public class ItemAmount : MonoBehaviour
{
    public UnityEvent OnAmountChangedToZero;
    public UlongEvent OnAmountChanged;

    ItemDef _itemDef;

    public ulong Amount = 1;
    ulong _reservedAmount = 0;

    void Awake()
    {
        _itemDef = GetComponent<ItemDefHolder>().ItemDef;
    }

    void Start()
    {
        SendAmountChangedEvent(Amount);
    }

    public ItemDef Def => _itemDef;

    public ulong Get() => Amount - _reservedAmount;

    public void Add(ulong amount)
    {
        Amount += amount;
        SendAmountChangedEvent(Amount);
    }

    public void Reserve(ulong amount)
    {
        Assert.IsTrue(amount <= Amount - _reservedAmount);

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
        Amount -= amount;
        SendAmountChangedEvent(Amount);
    }

    void SendAmountChangedEvent(ulong amount)
    {
        OnAmountChanged.Invoke(amount);
        if (amount == 0)
            OnAmountChangedToZero.Invoke();
    }
}
