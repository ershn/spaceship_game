using System;
using UnityEngine;

public interface ITask
{
    bool Started { get; }
    bool Executed { get; }
    bool Canceled { get; }
    bool? Succeeded { get; }
    bool RolledBack { get; }

    void Attach(GameObject executor);

    void Start();
    void Cancel();
    void Rollback();

    ITask Then(Action<bool> callback);
}
