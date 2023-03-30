using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BuildingLifecycle))]
[RequireComponent(typeof(BuildingComponents))]
[RequireComponent(typeof(ConstructionWork))]
public class BuildingConstructor : MonoBehaviour, IStateMachine
{
    class RequestComponents : State
    {
        readonly BuildingConstructor _constructor;
        readonly ItemRequestManager _itemRequestManager;
        readonly BuildingComponents _buildingComponents;

        Dictionary<ItemDef, ItemRequest> _requestedComponents;

        public RequestComponents(BuildingConstructor constructor)
            : base(constructor)
        {
            _constructor = constructor;
            _itemRequestManager = constructor.ItemRequestManager;
            _buildingComponents = constructor.GetComponent<BuildingComponents>();
        }

        protected override void OnStart() =>
            _buildingComponents.OnComponentMaxAmount.AddListener(Fulfill);

        protected override void OnEnd() =>
            _buildingComponents.OnComponentMaxAmount.RemoveListener(Fulfill);

        protected override void OnDo()
        {
            _requestedComponents = new();

            foreach (var component in _buildingComponents.GetRequiredAmounts())
            {
                var request = new ItemRequest(
                    component.ItemDef, component.Amount, _buildingComponents
                    );

                Debug.Log($"Request item: {request.ItemDef}, {request.Amount}");
                _requestedComponents[request.ItemDef] = request;
                _itemRequestManager.RequestItemDelivery(request);
            }

            if (_requestedComponents.Count == 0)
                ToState(new RequestConstruction(_constructor));
        }

        void Fulfill(ItemDef itemDef)
        {
            _requestedComponents.Remove(itemDef);

            if (_requestedComponents.Count == 0)
                ToState(new RequestConstruction(_constructor));
        }

        protected override void OnCancel()
        {
            foreach (var request in _requestedComponents.Values)
                _itemRequestManager.CancelItemDelivery(request);
            ToState(new CancelConstruction(_constructor));
        }
    }

    class RequestConstruction : State
    {
        readonly BuildingConstructor _constructor;
        readonly WorkRequestManager _workRequestManager;
        readonly ConstructionWork _constructionWork;

        public RequestConstruction(BuildingConstructor constructor)
            : base(constructor)
        {
            _constructor = constructor;
            _workRequestManager = constructor.WorkRequestManager;
            _constructionWork = constructor.GetComponent<ConstructionWork>();
        }

        protected override void OnStart() =>
            _constructionWork.OnWorkCompleted.AddListener(Complete);

        protected override void OnEnd() =>
            _constructionWork.OnWorkCompleted.RemoveListener(Complete);

        protected override void OnDo()
        {
            _workRequestManager.RequestWork(_constructionWork);
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
            _workRequestManager.CancelWork(_constructionWork);
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

    public ItemRequestManager ItemRequestManager;
    public WorkRequestManager WorkRequestManager;

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