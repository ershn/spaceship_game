using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BuildingDefHolder))]
[RequireComponent(typeof(GridPosition))]
public class BuildingComponents : MonoBehaviour, IInventoryAdd
{
    class Masses
    {
        public ulong MaxMass;
        public ulong CurrentMass;
    }

    public ItemDefEvent OnComponentMaxMass;

    public ItemCreator ItemCreator;

    BuildingDef _buildingDef;
    GridPosition _gridPosition;

    Dictionary<ItemDef, Masses> _inventory;

    void Awake()
    {
        _buildingDef = GetComponent<BuildingDefHolder>().BuildingDef;
        _gridPosition = GetComponent<GridPosition>();

        InitInventory();
    }

    void InitInventory()
    {
        _inventory = new();
        foreach (var componentMass in _buildingDef.ComponentMasses)
        {
            _inventory[componentMass.ItemDef] = new Masses()
            {
                MaxMass = componentMass.Mass,
                CurrentMass = 0
            };
        }
    }

    public IEnumerable<(ItemDef, ulong)> GetMissingComponents() =>
        _inventory.Select(kv => (kv.Key, kv.Value.MaxMass - kv.Value.CurrentMass));

    public void Add(ItemDef itemDef, ulong mass)
    {
        var component = _inventory[itemDef];

        if (component.CurrentMass + mass > component.MaxMass)
        {
            throw new ArgumentOutOfRangeException(
                "The total component mass would exceed the limit"
                );
        }

        component.CurrentMass += mass;

        if (component.CurrentMass == component.MaxMass)
            OnComponentMaxMass.Invoke(itemDef);
    }

    public void Dump()
    {
        var cellPosition = _gridPosition.CellPosition;

        foreach (var component in _inventory)
        {
            var itemDef = component.Key;
            var masses = component.Value;

            if (masses.CurrentMass > 0)
            {
                Debug.Log($"Dumping item: {cellPosition}, {itemDef}, {masses.CurrentMass}");
                ItemCreator.Upsert(cellPosition, itemDef, masses.CurrentMass);
                masses.CurrentMass = 0;
            }
        }
    }
}
