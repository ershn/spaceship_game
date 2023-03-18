using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BuildingDef))]
public class BuildingComponents : MonoBehaviour, IItemAmountAdd
{
    struct Amounts
    {
        public ulong MaxAmount;
        public ulong Amount;
    }

    public ItemDefEvent OnComponentMaxAmount;

    BuildingDef _buildingDef;

    Dictionary<ItemDef, Amounts> _inventory;

    void Awake()
    {
        _buildingDef = GetComponent<BuildingDefHolder>().BuildingDef;

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
                Amount = 0
            };
        }
    }

    public IEnumerable<ItemDefAmount> GetRequiredAmounts()
    {
        return _inventory.Select(kv =>
            new ItemDefAmount(kv.Key, kv.Value.MaxAmount - kv.Value.Amount)
        );
    }

    public void Add(ItemDef itemDef, ulong amount)
    {
        var component = _inventory[itemDef];

        if (component.Amount + amount > component.MaxAmount)
        {
            throw new ArgumentOutOfRangeException(
                "The total component amount would exceed the limit."
                );
        }

        component.Amount += amount;

        if (component.Amount == component.MaxAmount)
            OnComponentMaxAmount.Invoke(itemDef);
    }
}
