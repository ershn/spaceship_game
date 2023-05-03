using System;
using UnityEngine;

public class ItemFromWorldToBackpackTask : Task
{
    readonly ItemMass _item;
    readonly ulong _mass;
    IInventoryAdd _backpack;

    public ItemFromWorldToBackpackTask(ItemMass item, ulong mass)
    {
        _item = item;
        _mass = mass;

        Init();
    }

    void Init()
    {
        _item.Reserve(_mass);
    }

    public override void Attach(GameObject executor)
    {
        _backpack = executor.GetComponent<IInventoryAdd>();
    }

    // TODO: handle distance between item and executor
    protected override void OnStart(Action<bool> onEnd)
    {
        _item.Remove(_mass);
        _backpack.Add(_item.Def, _mass);
        onEnd(true);
    }

    protected override void OnFailure(bool executed)
    {
        _item.Unreserve(_mass);
    }
}
