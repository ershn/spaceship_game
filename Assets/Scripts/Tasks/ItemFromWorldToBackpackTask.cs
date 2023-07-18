using System;
using UnityEngine;

public class ItemFromWorldToBackpackTask : Task
{
    readonly ItemAmount _item;
    readonly ulong _amount;
    IInventoryAdd _backpack;

    public ItemFromWorldToBackpackTask(ItemAmount item, ulong amount)
    {
        _item = item;
        _amount = amount;

        Init();
    }

    void Init()
    {
        _item.Reserve(_amount);
    }

    public override void Attach(GameObject executor)
    {
        _backpack = executor.GetComponent<IInventoryAdd>();
    }

    // TODO: handle distance between item and executor
    protected override void OnStart(Action<bool> onEnd)
    {
        _item.Remove(_amount);
        _backpack.Add(_item.Def, _amount);
        onEnd(true);
    }

    protected override void OnFailure(bool executed)
    {
        // TODO: check if it is ok to unreserve an object that doesn't exist anymore
        _item.Unreserve(_amount);
    }
}
