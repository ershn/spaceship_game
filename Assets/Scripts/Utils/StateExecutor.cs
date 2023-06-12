using System;

using StateNode = Vertex<SuccessState, IState>;

public class StateExecutor
{
    StateNode _node;
    Action _onEnd;
    bool _canceled;

    public StateExecutor(StateNode startNode)
    {
        _node = startNode;
    }

    public void Start(Action onEnd = null)
    {
        _onEnd = onEnd;
        _node.State().Start(Next);
    }

    void Next(bool succeeded)
    {
        _node = _node[succeeded ? SuccessState.Success : SuccessState.Failure];
        if (_node != null)
            _node.State().Start(Next);
        else
            _onEnd?.Invoke();
    }

    public void Cancel()
    {
        if (_canceled || _node == null)
            return;

        _canceled = true;
        _node.State().Cancel();
    }
}
