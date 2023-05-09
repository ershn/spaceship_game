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

    public (T, ulong) First<T>() where T : ItemDef
    {
        var kv = _inventory.First();
        return ((T)kv.Key, kv.Value);
    }

    public bool TryFirst<T>(out (T, ulong) first) where T : ItemDef
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

    public void Add(ItemDef itemDef, ulong mass)
    {
        Assert.IsTrue(CurrentMass + mass <= MaxMass);

        if (_inventory.TryGetValue(itemDef, out var currentMass))
            _inventory[itemDef] = currentMass + mass;
        else
            _inventory[itemDef] = mass;

        CurrentMass += mass;
    }

    public void Remove(ItemDef itemDef, ulong mass)
    {
        var currentMass = _inventory[itemDef];
        Assert.IsTrue(mass <= currentMass);

        var updatedMass = currentMass - mass;
        if (updatedMass > 0)
            _inventory[itemDef] = updatedMass;
        else
            _inventory.Remove(itemDef);

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