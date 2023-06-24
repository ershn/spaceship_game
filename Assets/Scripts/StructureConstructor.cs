using static FunctionalUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using StateNode = Vertex<SuccessState, IState>;

[RequireComponent(typeof(ConstructionWork))]
[RequireComponent(typeof(Canceler))]
[RequireComponent(typeof(StructureComponents))]
public class StructureConstructor : MonoBehaviour
{
    class RequestComponents : IState
    {
        readonly ItemGridIndex _itemGrid;
        readonly TaskScheduler _taskScheduler;
        readonly StructureComponents _structureComponents;

        Action<bool> _onEnd;
        Dictionary<ItemDef, ITaskSet> _componentTaskSets;

        public RequestComponents(StructureConstructor constructor)
        {
            _itemGrid = constructor.transform.root.GetComponent<GridIndexes>().ItemGrid;
            _taskScheduler = constructor.TaskScheduler;
            _structureComponents = constructor.GetComponent<StructureComponents>();
        }

        public void Start(Action<bool> onEnd)
        {
            var unregister = _structureComponents.OnComponentMaxAmount.Register(Fulfill);
            _onEnd = Do(unregister, onEnd);
            Request();
        }

        void Request()
        {
            _componentTaskSets = new();

            foreach (var (itemDef, missingAmount) in _structureComponents.GetMissingComponents())
            {
                var taskSet = _itemGrid
                    .Filter(itemDef)
                    .CumulateAmount(missingAmount)
                    .Select(
                        item =>
                            TaskCreator.DeliverItem(
                                item.itemAmount,
                                item.markedAmount,
                                _structureComponents
                            )
                    )
                    .ToTaskSet();

                _componentTaskSets[itemDef] = taskSet;
                _taskScheduler.QueueTaskSet(taskSet);
            }

            if (!_componentTaskSets.Any())
                _onEnd(true);
        }

        void Fulfill(ItemDef itemDef)
        {
            _componentTaskSets.Remove(itemDef);

            if (!_componentTaskSets.Any())
                _onEnd(true);
        }

        public void Cancel()
        {
            foreach (var taskSet in _componentTaskSets.Values)
                taskSet.Cancel();

            _onEnd(false);
        }
    }

    class RequestConstruction : IState
    {
        readonly StructureConstructor _constructor;
        readonly TaskScheduler _taskScheduler;
        readonly ConstructionWork _constructionWork;

        Action<bool> _onEnd;
        ITask _task;

        public RequestConstruction(StructureConstructor constructor)
        {
            _constructor = constructor;
            _taskScheduler = constructor.TaskScheduler;
            _constructionWork = constructor.GetComponent<ConstructionWork>();
        }

        public void Start(Action<bool> onEnd)
        {
            var unregister = _constructionWork.OnWorkCompleted.Register(Fulfill);
            _onEnd = Do(unregister, onEnd);
            Request();
        }

        void Request()
        {
            _task = TaskCreator.WorkOn(_constructionWork);
            _taskScheduler.QueueTask(_task);
        }

        void Fulfill()
        {
            Destroy(_constructor);
            Destroy(_constructionWork);
            _onEnd(true);
        }

        public void Cancel()
        {
            _task.Cancel();
            _onEnd(false);
        }
    }

    class CancelConstruction : IState
    {
        readonly Destructor _destructor;

        public CancelConstruction(StructureConstructor constructor)
        {
            _destructor = constructor.GetComponent<Destructor>();
        }

        public void Start(Action<bool> onEnd)
        {
            _destructor.Destroy();
            onEnd(true);
        }

        public void Cancel() { }
    }

    public UnityEvent OnConstructionCompleted;
    public UnityEvent OnConstructionCanceled;

    public TaskScheduler TaskScheduler;

    Canceler _canceler;

    StateExecutor _stateExecutor;

    void Awake()
    {
        _canceler = GetComponent<Canceler>();
    }

    public void Construct()
    {
        var unregister = _canceler.OnCancel.Register(Cancel);
        _stateExecutor = new(StateGraph());
        _stateExecutor.Start(success =>
        {
            unregister();
            if (success)
                OnConstructionCompleted.Invoke();
            else
                OnConstructionCanceled.Invoke();
        });
    }

    void Cancel()
    {
        _stateExecutor?.Cancel();
    }

    StateNode StateGraph()
    {
        new StateNode(new RequestComponents(this), out var startNode)
            .Link(
                new(new RequestConstruction(this)),
                new(new CancelConstruction(this), out var failureNode)
            )
            .Link(null, failureNode);
        return startNode;
    }
}
