using System;

public interface IState
{
    void Start(Action<bool> onEnd);
    void Cancel();
}
