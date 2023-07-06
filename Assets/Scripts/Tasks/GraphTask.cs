using System;
using System.Collections.Generic;
using UnityEngine;

using TaskNode = Vertex<SuccessState, Task>;

public class GraphTask : Task
{
    readonly TaskNode _startNode;
    readonly HashSet<TaskNode> _allNodes;
    Action<bool> _onEnd;
    Task _currentTask;
    bool _failed;

    public GraphTask(TaskNode startNode)
    {
        _startNode = startNode;
        _allNodes = startNode.AllLinkedVertices();
    }

    public override void Attach(GameObject executor)
    {
        foreach (var node in _allNodes)
            node.Task().Attach(executor);
    }

    protected override void OnStart(Action<bool> onEnd)
    {
        _onEnd = onEnd;
        ExecuteNode(_startNode);
    }

    void ExecuteNode(TaskNode node)
    {
        _currentTask = node.Task();
        _currentTask.Then(success =>
        {
            TaskNode nextNode;

            if (success)
            {
                nextNode = node.SuccessNode();
                node.DeepUnlink(SuccessState.Failure, node => node.Task().Cancel());
            }
            else
            {
                _failed = true;
                nextNode = node.FailureNode();
                node.DeepUnlink(SuccessState.Success, node => node.Task().Cancel());
            }

            if (nextNode != null)
                ExecuteNode(nextNode);
            else
                _onEnd(!_failed);
        });
        _currentTask.Start();
    }

    protected override void OnCancel()
    {
        _currentTask.Cancel();
    }

    protected override void OnFailure(bool executed)
    {
        if (executed)
            return;

        foreach (var node in _allNodes)
            node.Task().Cancel();
    }
}
