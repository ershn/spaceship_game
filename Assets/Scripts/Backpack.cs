using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Backpack : MonoBehaviour, IInventoryAdd, IInventoryRemove
{
    ItemCreator _itemCreator;
    GridPosition _gridPosition;

    [SerializeField, Amount(AmountType.Mass)]
    ulong _maxMass = 100.KiloGrams();

    public ulong CurrentMass { get; private set; }

    Dictionary<ItemDef, ulong> _inventory = new();

    void Awake()
    {
        _itemCreator = GetComponentInParent<WorldInternalIO>().ItemCreator;
        _gridPosition = GetComponent<GridPosition>();
    }

    public (T, ulong) First<T>()
        where T : ItemDef
    {
        var (itemDef, amount) = _inventory.First();
        return ((T)itemDef, amount);
    }

    public bool TryFirst<T>(out (T, ulong) first)
        where T : ItemDef
    {
        if (_inventory.Any())
        {
            first = First<T>();
            return true;
        }
        else
        {
            first = default;
            return false;
        }
    }

    public void Add(ItemDef itemDef, ulong amount)
    {
        var mass = itemDef.AmountMode.AmountToMass(amount);
        Assert.IsTrue(CurrentMass + mass <= _maxMass);

        if (_inventory.TryGetValue(itemDef, out var currentAmount))
            _inventory[itemDef] = currentAmount + amount;
        else
            _inventory[itemDef] = amount;

        CurrentMass += mass;
    }

    public void Remove(ItemDef itemDef, ulong amount)
    {
        var currentAmount = _inventory[itemDef];
        Assert.IsTrue(amount <= currentAmount);

        var updatedAmount = currentAmount - amount;
        if (updatedAmount > 0)
            _inventory[itemDef] = updatedAmount;
        else
            _inventory.Remove(itemDef);

        var mass = itemDef.AmountMode.AmountToMass(amount);
        CurrentMass -= mass;
    }

    public void Dump()
    {
        foreach (var item in _inventory)
            _itemCreator.Create(_gridPosition.CellPosition, item.Key, item.Value);

        CurrentMass = 0;
        _inventory = new();
    }
}
