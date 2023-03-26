using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BuildingDefHolder))]
[RequireComponent(typeof(GridPosition))]
public class BuildingComponents : MonoBehaviour, IItemAmountAdd
{
    class Amounts
    {
        public ulong MaxAmount;
        public ulong CurrentAmount;
    }

    public ItemDefEvent OnComponentMaxAmount;

    public ItemCreator ItemCreator;

    BuildingDef _buildingDef;
    GridPosition _gridPosition;

    Dictionary<ItemDef, Amounts> _inventory;

    void Awake()
    {
        _buildingDef = GetComponent<BuildingDefHolder>().BuildingDef;
        _gridPosition = GetComponent<GridPosition>();

        InitInventory();
    }

    void InitInventory()
    {
        _inventory = new();
        foreach (var componentAmount in _buildingDef.ComponentAmounts)
        {
            _inventory[componentAmount.ItemDef] = new Amounts()
            {
                MaxAmount = componentAmount.Amount,
                CurrentAmount = 0
            };
        }
    }

    public IEnumerable<ItemDefAmount> GetRequiredAmounts()
    {
        return _inventory.Select(kv =>
            new ItemDefAmount(kv.Key, kv.Value.MaxAmount - kv.Value.CurrentAmount)
        );
    }

    public void Add(ItemDef itemDef, ulong amount)
    {
        var component = _inventory[itemDef];

        if (component.CurrentAmount + amount > component.MaxAmount)
        {
            throw new ArgumentOutOfRangeException(
                "The total component amount would exceed the limit."
                );
        }

        component.CurrentAmount += amount;

        if (component.CurrentAmount == component.MaxAmount)
            OnComponentMaxAmount.Invoke(itemDef);
    }

    public void Dump()
    {
        var cellPosition = _gridPosition.CellPosition;

        foreach (var component in _inventory)
        {
            var itemDef = component.Key;
            var amounts = component.Value;

            if (amounts.CurrentAmount > 0)
            {
                Debug.Log($"Dumping item: {cellPosition}, {itemDef}, {amounts.CurrentAmount}");
                ItemCreator.Upsert(cellPosition, itemDef, amounts.CurrentAmount);
                amounts.CurrentAmount = 0;
            }
        }
    }
}
