using System;
using UnityEngine;

public class MoveTask : ITask
{
    PathFinder _pathFinder;
    Vector2 _dest;

    public MoveTask(Vector2 dest)
    {
        _dest = dest;
    }

    public void Setup(GameObject executor)
    {
        _pathFinder = executor.GetComponent<PathFinder>();
    }

    public void Start(Action<bool> onEnd)
    {
        _pathFinder.MoveTo(_dest, onEnd);
    }
}
