using System.Collections.Generic;
using UnityEngine;

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
        readonly ConstructionRequestManager _constructionRequestManager;
        readonly ConstructionWork _constructionWork;

        public RequestConstruction(BuildingConstructor constructor)
            : base(constructor)
        {
            _constructor = constructor;
            _constructionRequestManager = constructor.ConstructionRequestManager;
            _constructionWork = constructor.GetComponent<ConstructionWork>();
        }

        protected override void OnStart() =>
            _constructionWork.OnConstructionCompleted.AddListener(Complete);

        protected override void OnEnd() =>
            _constructionWork.OnConstructionCompleted.RemoveListener(Complete);

        protected override void OnDo()
        {
            _constructionRequestManager.RequestConstruction(_constructionWork);
        }

        void Complete()
        {
            ToEnded();
        }

        protected override void OnCancel()
        {
            _constructionRequestManager.CancelConstruction(_constructionWork);
            ToState(new CancelConstruction(_constructor));
        }
    }

    class CancelConstruction : State
    {
        readonly BuildingConstructor _constructor;
        readonly BuildingComponents _buildingComponents;

        public CancelConstruction(BuildingConstructor constructor)
            : base(constructor)
        {
            _constructor = constructor;
            _buildingComponents = constructor.GetComponent<BuildingComponents>();
        }

        protected override void OnDo()
        {
            _buildingComponents.Dump();
            Destroy(_constructor.gameObject);
            ToEnded();
        }

        protected override void OnCancel()
        {
            ToEnded();
        }
    }

    public ItemRequestManager ItemRequestManager;
    public ConstructionRequestManager ConstructionRequestManager;

    State _state;
    bool _ended;

    void Start()
    {
        ToState(new RequestComponents(this));
    }

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

    public void Cancel()
    {
        if (!_ended)
            _state.Cancel();
    }
}