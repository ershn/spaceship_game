using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphTask : Task
{
    enum Arc
    {
        Success,
        Failure
    }

    TaskNode _startNode;
    HashSet<TaskNode> _allNodes;
    Action<bool> _onEnd;
    ITask _currentTask;
    bool _failed;

    public GraphTask(TaskNode startNode)
    {
        Init(startNode);
    }

    void Init(TaskNode startNode)
    {
        _startNode = startNode;
        _allNodes = new();

        void OnNode(TaskNode node)
        {
            _allNodes.Add(node);
            if (node.SuccessNode != null)
                OnNode(node.SuccessNode);
            if (node.FailureNode != null)
                OnNode(node.FailureNode);
        }

        OnNode(startNode);
    }

    public override void Attach(GameObject executor)
    {
        foreach (var node in _allNodes)
            node.Task.Attach(executor);
    }

    protected override void OnStart(Action<bool> onEnd)
    {
        _onEnd = onEnd;
        ExecuteNode(_startNode);
    }

    void ExecuteNode(TaskNode node)
    {
        _currentTask = node.Task;
        _currentTask.Then(success =>
        {
            TaskNode nextNode;

            if (success)
            {
                nextNode = node.SuccessNode;
                if (node.FailureNode != null)
                    RemoveLeadingArc(node.FailureNode, Arc.Failure);
            }
            else
            {
                _failed = true;
                nextNode = node.FailureNode;
                if (node.SuccessNode != null)
                    RemoveLeadingArc(node.SuccessNode, Arc.Success);
            }

            if (nextNode != null)
                ExecuteNode(nextNode);
            else
                _onEnd(!_failed);
        });
        _currentTask.Start();
    }

    void RemoveLeadingArc(TaskNode node, Arc arc)
    {
        if (arc == Arc.Success)
            node.LeadingSuccessArcs--;
        else
            node.LeadingFailureArcs--;

        if (node.LeadingSuccessArcs > 0 || node.LeadingFailureArcs > 0)
            return;

        node.Task.Cancel();

        if (node.SuccessNode != null)
            RemoveLeadingArc(node.SuccessNode, Arc.Success);
        if (node.FailureNode != null)
            RemoveLeadingArc(node.FailureNode, Arc.Failure);
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
            node.Task.Cancel();
    }
}