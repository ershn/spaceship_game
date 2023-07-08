using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class StructureComponents : MonoBehaviour, IInventoryAdd
{
    class ComponentAmount
    {
        public ulong RequiredAmount;
        public ulong CurrentAmount;
    }

    public UnityEvent OnFull;

    StructureDef _structureDef;
    GridPosition _gridPosition;
    ItemCreator _itemCreator;

    readonly Dictionary<ItemDef, ComponentAmount> _components = new();
    uint _missingComponents = 0;

    void Awake()
    {
        _structureDef = GetComponent<StructureDefHolder>().StructureDef;
        _gridPosition = GetComponent<GridPosition>();
        _itemCreator = transform.root.GetComponent<WorldInternalIO>().ItemCreator;

        InitInventory();
        GetComponent<Destructor>().OnDestruction.AddListener(DumpInventory);
    }

    void InitInventory()
    {
        foreach (var componentAmount in _structureDef.ComponentAmounts)
        {
            _components[componentAmount.ItemDef] = new()
            {
                RequiredAmount = componentAmount.Amount,
                CurrentAmount = 0
            };
            _missingComponents++;
        }
    }

    public bool Full => _missingComponents == 0;

    public IEnumerable<(ItemDef, ulong)> GetMissing() =>
        _components
            .Where(kv => kv.Value.CurrentAmount < kv.Value.RequiredAmount)
            .Select(kv => (kv.Key, kv.Value.RequiredAmount - kv.Value.CurrentAmount));

    public void Add(ItemDef itemDef, ulong amount)
    {
        Assert.IsTrue(amount > 0);

        var component = _components[itemDef];
        Assert.IsTrue(component.CurrentAmount + amount <= component.RequiredAmount);

        component.CurrentAmount += amount;
        if (component.CurrentAmount == component.RequiredAmount)
            _missingComponents--;

        if (_missingComponents == 0)
            OnFull.Invoke();
    }

    void DumpInventory()
    {
        var cellPosition = _gridPosition.CellPosition;

        foreach (var (itemDef, amounts) in _components)
        {
            if (amounts.CurrentAmount == 0)
                continue;

            _itemCreator.Create(cellPosition, itemDef, amounts.CurrentAmount);
            amounts.CurrentAmount = 0;
            _missingComponents++;
        }
    }
}
