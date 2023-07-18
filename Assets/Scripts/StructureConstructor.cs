using static FunctionalUtils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using StateNode = Vertex<SuccessState, IState>;

public class StructureConstructor : MonoBehaviour
{
    class RequestComponents : IState
    {
        readonly ItemAllotter _itemAllotter;
        readonly StructureComponentInventory _componentInventory;

        Action<bool> _onEnd;
        readonly List<ItemAllotter.IRequest> _requests = new();

        public RequestComponents(StructureConstructor constructor)
        {
            _itemAllotter = constructor.transform.root.GetComponent<WorldInternalIO>().ItemAllotter;
            _componentInventory = constructor.GetComponent<StructureComponentInventory>();
        }

        public void Start(Action<bool> onEnd)
        {
            if (_componentInventory.Full)
            {
                onEnd(true);
                return;
            }

            var unregister = _componentInventory.OnFull.Register(_ => Complete());
            _onEnd = Do(unregister, onEnd);

            foreach (var (itemDef, missingAmount) in _componentInventory.UnfilledSlots())
            {
                var request = _itemAllotter.Request(itemDef, missingAmount, _componentInventory);
                _requests.Add(request);
            }
        }

        void Complete()
        {
            _onEnd(true);
        }

        public void Cancel()
        {
            foreach (var request in _requests)
                request.Cancel();

            _onEnd(false);
        }
    }

    class RequestConstruction : IState
    {
        readonly StructureConstructor _constructor;
        readonly TaskScheduler _taskScheduler;
        readonly ConstructionWork _constructionWork;

        Action<bool> _onEnd;
        Task _task;

        public RequestConstruction(StructureConstructor constructor)
        {
            var root = constructor.transform.root;
            _constructor = constructor;
            _taskScheduler = root.GetComponent<WorldInternalIO>().TaskScheduler;
            _constructionWork = constructor.GetComponent<ConstructionWork>();
        }

        public void Start(Action<bool> onEnd)
        {
            var unregister = _constructionWork.OnWorkCompleted.Register(Complete);
            _onEnd = Do(unregister, onEnd);

            _task = TaskCreator.WorkOn(_constructionWork);
            _taskScheduler.QueueTask(_task);
        }

        void Complete()
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

    StateExecutor _stateExecutor;

    public void Construct()
    {
        var unregister = GetComponent<Canceler>().OnCancel.Register(Cancel);

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
