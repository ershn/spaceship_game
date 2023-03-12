using System;
using UnityEngine;

public class InventoryToInventoryTask : Task
{
    IItemAmountAdd _targetInventory;
    ItemDef _itemDef;
    ulong _amount;
    IItemAmountRemove _sourceInventory;

    public InventoryToInventoryTask(
        IItemAmountAdd targetInventory, ItemDef itemDef, ulong amount
        )
    {
        _targetInventory = targetInventory;
        _itemDef = itemDef;
        _amount = amount;
    }

    public override void Attach(GameObject executor)
    {
        _sourceInventory = executor.GetComponent<IItemAmountRemove>();
    }

    // TODO: handle distance between item and executor
    protected override void OnStart(Action<bool> onEnd)
    {
        _sourceInventory.Remove(_itemDef, _amount);
        _targetInventory.Add(_itemDef, _amount);
        onEnd(true);
    }
}
