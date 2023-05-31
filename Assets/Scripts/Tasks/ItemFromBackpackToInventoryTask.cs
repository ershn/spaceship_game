using System;
using UnityEngine;

public class ItemFromBackpackToInventoryTask : Task
{
    readonly IInventoryAdd _inventory;
    readonly ItemDef _itemDef;
    readonly ulong _amount;
    IInventoryRemove _backpack;

    public ItemFromBackpackToInventoryTask(IInventoryAdd inventory, ItemDef itemDef, ulong amount)
    {
        _inventory = inventory;
        _itemDef = itemDef;
        _amount = amount;
    }

    public override void Attach(GameObject executor)
    {
        _backpack = executor.GetComponent<IInventoryRemove>();
    }

    protected override void OnStart(Action<bool> onEnd)
    {
        _backpack.Remove(_itemDef, _amount);
        _inventory.Add(_itemDef, _amount);
        onEnd(true);
    }
}
