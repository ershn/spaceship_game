using System;
using UnityEngine;

public class MoveTask : Task
{
    Vector2 _dest;
    PathFinder _pathFinder;

    public MoveTask(Vector2 dest)
    {
        _dest = dest;
    }

    public override void Prepare(GameObject executor)
    {
        _pathFinder = executor.GetComponent<PathFinder>();
    }

    protected override void OnStart(Action<bool> onEnd)
    {
        _pathFinder.MoveTo(_dest, onEnd);
    }

    protected override void OnCancel()
    {
        _pathFinder.Cancel();
    }
}
