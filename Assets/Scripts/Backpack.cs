using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Backpack : MonoBehaviour, IInventoryAdd, IInventoryRemove
{
    public ItemCreator ItemCreator;

    GridPosition _gridPosition;

    public ulong MaxMass = 100.KiloGrams();
    public ulong CurrentMass { get; private set; }

    Dictionary<ItemDef, ulong> _inventory = new();

    void Awake()
    {
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
        var mass = itemDef.AmountAddressingMode.AmountToMass(amount);
        Assert.IsTrue(CurrentMass + mass <= MaxMass);

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

        var mass = itemDef.AmountAddressingMode.AmountToMass(amount);
        CurrentMass -= mass;
    }

    public void Dump()
    {
        foreach (var item in _inventory)
            ItemCreator.Upsert(_gridPosition.CellPosition, item.Key, item.Value);

        CurrentMass = 0;
        _inventory = new();
    }
}