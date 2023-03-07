using System;
using UnityEngine;

public interface ITask
{
    void Setup(GameObject executor);
    void Start(Action<bool> onEnd);
}
