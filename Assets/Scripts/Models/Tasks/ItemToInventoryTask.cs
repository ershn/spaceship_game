using System;
using UnityEngine;

public class ItemToInventoryTask : Task
{
    IAmountRemove _item;
    ItemDef _itemDef;
    ulong _amount;
    IItemAmountAdd _inventory;

    public ItemToInventoryTask(
        IAmountRemove item, ItemDef itemDef, ulong amount
        )
    {
        _item = item;
        _itemDef = itemDef;
        _amount = amount;
    }

    public override void Prepare(GameObject executor)
    {
        _inventory = executor.GetComponent<IItemAmountAdd>();
    }

    // TODO: handle distance between item and executor
    protected override void OnStart(Action<bool> onEnd)
    {
        _item.Remove(_amount);
        _inventory.Add(_itemDef, _amount);
        onEnd(true);
    }
}
