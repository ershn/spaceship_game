using System;

public abstract class State
{
    readonly IStateMachine _stateMachine;

    public State(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Do()
    {
        OnStart();
        OnDo();
    }

    protected abstract void OnDo();

    public void Cancel()
    {
        OnCancel();
    }

    protected virtual void OnCancel()
    {
        throw new NotImplementedException("OnCancel not implemented for this State");
    }

    protected virtual void OnStart() { }

    protected virtual void OnEnd() { }

    protected void ToState(State state)
    {
        OnEnd();
        _stateMachine.ToState(state);
    }

    protected void ToEnded()
    {
        OnEnd();
        _stateMachine.ToEnded();
    }
}
