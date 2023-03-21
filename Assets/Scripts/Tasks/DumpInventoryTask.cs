using System;
using UnityEngine;

public class DumpInventoryTask : Task
{
    ItemCreator _itemCreator;
    GridPosition _gridPosition;
    Inventory _inventory;

    public DumpInventoryTask(ItemCreator itemCreator)
    {
        _itemCreator = itemCreator;
    }

    public override void Attach(GameObject executor)
    {
        _gridPosition = executor.GetComponent<GridPosition>();
        _inventory = executor.GetComponent<Inventory>();
    }

    protected override void OnStart(Action<bool> onEnd)
    {
        foreach (var item in _inventory.Empty())
            _itemCreator.Upsert(_gridPosition.CellPosition, item.ItemDef, item.Amount);

        onEnd(true);
    }
}
