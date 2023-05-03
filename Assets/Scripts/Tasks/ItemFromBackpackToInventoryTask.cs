using System;
using UnityEngine;

public class ItemFromBackpackToInventoryTask : Task
{
    readonly IInventoryAdd _inventory;
    readonly ItemDef _itemDef;
    readonly ulong _mass;
    IInventoryRemove _backpack;

    public ItemFromBackpackToInventoryTask(
        IInventoryAdd inventory, ItemDef itemDef, ulong mass
        )
    {
        _inventory = inventory;
        _itemDef = itemDef;
        _mass = mass;
    }

    public override void Attach(GameObject executor)
    {
        _backpack = executor.GetComponent<IInventoryRemove>();
    }

    protected override void OnStart(Action<bool> onEnd)
    {
        _backpack.Remove(_itemDef, _mass);
        _inventory.Add(_itemDef, _mass);
        onEnd(true);
    }
}
