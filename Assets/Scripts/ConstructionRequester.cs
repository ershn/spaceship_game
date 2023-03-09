using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingComponents))]
[RequireComponent(typeof(ConstructionWork))]
public class ConstructionRequester : MonoBehaviour
{
    enum Phase
    {
        RequestingComponents,
        RequestingConstruction,
        Done,
        Canceled
    }

    BuildingComponents _buildingComponents;
    ConstructionWork _constructionWork;

    Phase _phase;

    void Awake()
    {
        _buildingComponents = GetComponent<BuildingComponents>();
        _constructionWork = GetComponent<ConstructionWork>();
    }

    void Start()
    {
        StartConstruction();
    }

    void StartConstruction()
    {
        if (!RequestComponents())
            return;
        RequestConstruction();
    }

#region component requesting

    Dictionary<ItemDef, ItemRequest> _requestedComponents;

    bool RequestComponents()
    {
        _phase = Phase.RequestingComponents;
        _requestedComponents = new();

        foreach (var component in _buildingComponents.GetRequiredAmounts())
        {
            var request = new ItemRequest(
                component.ItemDef, component.Amount, _buildingComponents
                );

            Debug.Log($"Request item: {request.ItemDef}, {request.Amount}");

            _requestedComponents[request.ItemDef] = request;
            ItemRequestManager.Instance.RequestItemDelivery(request);
        }

        return _requestedComponents.Count == 0;
    }

    public void FulfillComponentRequest(ItemDef itemDef)
    {
        _requestedComponents.Remove(itemDef);

        if (_requestedComponents.Count == 0)
            RequestConstruction();
    }

    void CancelComponentRequests()
    {
        _phase = Phase.Canceled;
        foreach (var request in _requestedComponents.Values)
            ItemRequestManager.Instance.CancelItemDelivery(request);
    }

#endregion
#region construction requesting

    void RequestConstruction()
    {
        _phase = Phase.RequestingConstruction;
        ConstructionRequestManager.Instance.RequestConstruction(_constructionWork);
    }

    public void FulfillConstructionRequest()
    {
        CompleteConstruction();
    }

    void CancelConstructionRequest()
    {
        _phase = Phase.Canceled;
        ConstructionRequestManager.Instance.CancelConstruction(_constructionWork);
    }

#endregion

    void CompleteConstruction()
    {
        _phase = Phase.Done;
    }

    public void CancelConstruction()
    {
        if (_phase == Phase.RequestingComponents)
            CancelComponentRequests();
        else if (_phase == Phase.RequestingConstruction)
            CancelConstructionRequest();
    }
}