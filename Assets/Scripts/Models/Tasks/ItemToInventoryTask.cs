using System;
using UnityEngine;

public class ItemToInventoryTask : ITask
{
    IAmountRemove _item;
    IItemAmountAdd _inventory;
    ItemDef _itemDef;
    ulong _amount;

    public ItemToInventoryTask(
        IAmountRemove item, ItemDef itemDef, ulong amount
        )
    {
        _item = item;
        _itemDef = itemDef;
        _amount = amount;
    }

    public void Setup(GameObject executor)
    {
        _inventory = executor.GetComponent<IItemAmountAdd>();
    }

    // TODO: handle distance between item and executor
    public void Start(Action<bool> onEnd)
    {
        _item.Remove(_amount);
        _inventory.Add(_itemDef, _amount);
        onEnd(true);
    }
}
