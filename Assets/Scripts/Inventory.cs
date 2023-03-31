using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IItemAmountAdd, IItemAmountRemove
{
    public ItemCreator ItemCreator;

    GridPosition _gridPosition;

    public ulong MaxAmount = 100.KiloGrams();
    public ulong CurrentAmount { get; private set; }

    Dictionary<ItemDef, ulong> _inventory = new();

    void Awake()
    {
        _gridPosition = GetComponent<GridPosition>();
    }

    public void Add(ItemDef itemDef, ulong amount)
    {
        if (CurrentAmount + amount > MaxAmount)
        {
            throw new ArgumentOutOfRangeException(
                "The total inventory amount would exceed the limit."
                );
        }

        if (_inventory.TryGetValue(itemDef, out var currentAmount))
            _inventory[itemDef] = currentAmount + amount;
        else
            _inventory[itemDef] = amount;

        CurrentAmount += amount;
    }

    public void Remove(ItemDef itemDef, ulong amount)
    {
        var currentAmount = _inventory[itemDef];

        if (amount > currentAmount)
        {
            throw new ArgumentOutOfRangeException(
                "The removed amount exceeds the current amount."
                );
        }

        _inventory[itemDef] = currentAmount - amount;

        CurrentAmount -= amount;
    }

    public void Dump()
    {
        foreach (var item in _inventory)
            ItemCreator.Upsert(_gridPosition.CellPosition, item.Key, item.Value);

        CurrentAmount = 0;
        _inventory = new();
    }
}