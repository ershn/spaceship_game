using System;
using UnityEngine;

public class InventoryToInventoryTask : ITask
{
    IItemAmountRemove _sourceInventory;
    IItemAmountAdd _targetInventory;
    ItemDef _itemDef;
    ulong _amount;

    public InventoryToInventoryTask(
        IItemAmountAdd targetInventory, ItemDef itemDef, ulong amount
        )
    {
        _targetInventory = targetInventory;
        _itemDef = itemDef;
        _amount = amount;
    }

    public void Setup(GameObject executor)
    {
        _sourceInventory = executor.GetComponent<IItemAmountRemove>();
    }

    public void Start(Action<bool> onEnd)
    {
        _sourceInventory.Remove(_itemDef, _amount);
        _targetInventory.Add(_itemDef, _amount);
        onEnd(true);
    }
}
