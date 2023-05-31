using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BuildingLifecycle))]
[RequireComponent(typeof(BuildingComponents))]
[RequireComponent(typeof(ConstructionWork))]
public class BuildingConstructor : MonoBehaviour, IStateMachine
{
    class RequestComponents : State
    {
        readonly ItemGridIndexer _itemGrid;
        readonly TaskScheduler _taskScheduler;

        readonly BuildingConstructor _constructor;
        readonly BuildingComponents _buildingComponents;

        Dictionary<ItemDef, ITaskSet> _componentTaskSets;

        public RequestComponents(BuildingConstructor constructor)
            : base(constructor)
        {
            _itemGrid = constructor.ItemGrid;
            _taskScheduler = constructor.TaskScheduler;

            _constructor = constructor;
            _buildingComponents =
                constructor.GetComponent<BuildingComponents>();
        }

        protected override void OnStart() =>
            _buildingComponents.OnComponentMaxAmount.AddListener(Fulfill);

        protected override void OnEnd() =>
            _buildingComponents.OnComponentMaxAmount.RemoveListener(Fulfill);

        protected override void OnDo()
        {
            _componentTaskSets = new();

            foreach (var (itemDef, missingAmount) in
                        _buildingComponents.GetMissingComponents())
            {
                var taskSet = _itemGrid
                .Filter(itemDef)
                .CumulateAmount(missingAmount)
                .Select(item =>
                    TaskCreator.DeliverItem(
                        item.itemAmount, item.markedAmount, _buildingComponents
                        )
                    )
                .ToTaskSet();

                _componentTaskSets[itemDef] = taskSet;
                _taskScheduler.QueueTaskSet(taskSet);
            }

            if (_componentTaskSets.Count == 0)
                ToState(new RequestConstruction(_constructor));
        }

        void Fulfill(ItemDef itemDef)
        {
            _componentTaskSets.Remove(itemDef);

            if (_componentTaskSets.Count == 0)
                ToState(new RequestConstruction(_constructor));
        }

        protected override void OnCancel()
        {
            foreach (var taskSet in _componentTaskSets.Values)
                taskSet.Cancel();
            ToState(new CancelConstruction(_constructor));
        }
    }

    class RequestConstruction : State
    {
        readonly TaskScheduler _taskScheduler;

        readonly BuildingConstructor _constructor;
        readonly ConstructionWork _constructionWork;

        ITask _task;

        public RequestConstruction(BuildingConstructor constructor)
            : base(constructor)
        {
            _taskScheduler = constructor.TaskScheduler;

            _constructor = constructor;
            _constructionWork = constructor.GetComponent<ConstructionWork>();
        }

        protected override void OnStart() =>
            _constructionWork.OnWorkCompleted.AddListener(Complete);

        protected override void OnEnd() =>
            _constructionWork.OnWorkCompleted.RemoveListener(Complete);

        protected override void OnDo()
        {
            _task = TaskCreator.WorkOn(_constructionWork);
            _taskScheduler.QueueTask(_task);
        }

        void Complete()
        {
            Destroy(_constructor);
            Destroy(_constructionWork);
            _constructor.OnConstructionCompleted.Invoke();
            ToEnded();
        }

        protected override void OnCancel()
        {
            _task.Cancel();
            ToState(new CancelConstruction(_constructor));
        }
    }

    class CancelConstruction : State
    {
        readonly BuildingLifecycle _lifecycle;

        public CancelConstruction(BuildingConstructor constructor)
            : base(constructor)
        {
            _lifecycle = constructor.GetComponent<BuildingLifecycle>();
        }

        protected override void OnDo()
        {
            _lifecycle.Destroy();
            ToEnded();
        }
    }

    public UnityEvent OnConstructionCompleted;

    public ItemGridIndexer ItemGrid;
    public TaskScheduler TaskScheduler;

    State _state;
    bool _ended;
    bool _canceled;

    public void ToState(State state)
    {
        _state = state;
        _state.Do();
    }

    public void ToEnded()
    {
        _ended = true;
        _state = null;
    }

    public void Construct()
    {
        ToState(new RequestComponents(this));
    }

    public void Cancel()
    {
        if (!_ended && !_canceled)
        {
            _canceled = true;
            _state.Cancel();
        }
    }
}