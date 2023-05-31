using System;
using UnityEngine;

public class DumpBackpackTask : Task
{
    Backpack _backpack;

    public override void Attach(GameObject executor)
    {
        _backpack = executor.GetComponent<Backpack>();
    }

    protected override void OnStart(Action<bool> onEnd)
    {
        _backpack.Dump();
        onEnd(true);
    }
}
