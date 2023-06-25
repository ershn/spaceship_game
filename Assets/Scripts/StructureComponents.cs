using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class StructureComponents : MonoBehaviour, IInventoryAdd
{
    class Amounts
    {
        public ulong MaxAmount;
        public ulong CurrentAmount;
    }

    public ItemDefEvent OnComponentMaxAmount;

    StructureDef _structureDef;
    GridPosition _gridPosition;
    ItemCreator _itemCreator;

    readonly Dictionary<ItemDef, Amounts> _inventory = new();

    void Awake()
    {
        _structureDef = GetComponent<StructureDefHolder>().StructureDef;
        _gridPosition = GetComponent<GridPosition>();
        _itemCreator = transform.root.GetComponent<WorldInternalIO>().ItemCreator;

        Init();
        GetComponent<Destructor>().OnDestruction.AddListener(Dump);
    }

    void Init()
    {
        foreach (var componentAmount in _structureDef.ComponentAmounts)
        {
            _inventory[componentAmount.ItemDef] = new Amounts()
            {
                MaxAmount = componentAmount.Amount,
                CurrentAmount = 0
            };
        }
    }

    public IEnumerable<(ItemDef, ulong)> GetMissingComponents() =>
        _inventory.Select(kv => (kv.Key, kv.Value.MaxAmount - kv.Value.CurrentAmount));

    public void Add(ItemDef itemDef, ulong amount)
    {
        var component = _inventory[itemDef];
        Assert.IsTrue(component.CurrentAmount + amount <= component.MaxAmount);

        component.CurrentAmount += amount;

        if (component.CurrentAmount == component.MaxAmount)
            OnComponentMaxAmount.Invoke(itemDef);
    }

    void Dump()
    {
        var cellPosition = _gridPosition.CellPosition;

        foreach (var (itemDef, amounts) in _inventory)
        {
            if (amounts.CurrentAmount == 0)
                continue;

            _itemCreator.Upsert(cellPosition, itemDef, amounts.CurrentAmount);
            amounts.CurrentAmount = 0;
        }
    }
}
