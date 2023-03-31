using System;
using UnityEngine;

public class DumpInventoryTask : Task
{
    Inventory _inventory;

    public override void Attach(GameObject executor)
    {
        _inventory = executor.GetComponent<Inventory>();
    }

    protected override void OnStart(Action<bool> onEnd)
    {
        _inventory.Dump();
        onEnd(true);
    }
}