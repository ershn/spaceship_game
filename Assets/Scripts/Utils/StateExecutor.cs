using StateNode = Vertex<SuccessState, IState>;

public class StateExecutor
{
    StateNode _node;
    bool _canceled;

    public StateExecutor(StateNode startNode)
    {
        _node = startNode;
    }

    public void Start()
    {
        _node?.Value.Start(Next);
    }

    void Next(bool succeeded)
    {
        _node = _node[succeeded ? SuccessState.Success : SuccessState.Failure];
        Start();
    }

    public void Cancel()
    {
        if (_canceled || _node == null)
            return;

        _canceled = true;
        _node.Value.Cancel();
    }
}
