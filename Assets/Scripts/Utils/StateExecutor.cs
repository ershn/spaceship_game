using System;

using StateNode = Vertex<SuccessState, IState>;

public class StateExecutor
{
    StateNode _node;
    Action<bool> _onEnd;
    bool _failed;
    bool _canceled;

    public StateExecutor(StateNode startNode)
    {
        _node = startNode;
    }

    public void Start(Action<bool> onEnd = null)
    {
        _onEnd = onEnd;
        _node.State().Start(Next);
    }

    void Next(bool success)
    {
        if (success)
            _node = _node[SuccessState.Success];
        else
        {
            _failed = true;
            _node = _node[SuccessState.Failure];
        }

        if (_node != null)
            _node.State().Start(Next);
        else
            _onEnd?.Invoke(!_failed);
    }

    public void Cancel()
    {
        if (_canceled || _node == null)
            return;

        _canceled = true;
        _node.State().Cancel();
    }
}
