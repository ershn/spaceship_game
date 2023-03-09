using System;
using UnityEngine;

public interface ITask
{
    void Prepare(GameObject executor);

    void Start();
    void Cancel();

    ITask Then(Action<bool> callback);
}
