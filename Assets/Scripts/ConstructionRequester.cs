using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingComponents))]
public class ConstructionRequester : MonoBehaviour
{
    public static event Action<ItemDef, ulong, IItemAmountAdd> OnItemRequest;
    public static event Action<IWork> OnConstructionRequest;

    BuildingComponents _buildingComponents;
    ConstructionWork _constructionWork;

    HashSet<ItemDef> _requestedComponents;

    void Awake()
    {
        _buildingComponents = GetComponent<BuildingComponents>();
        _constructionWork = GetComponent<ConstructionWork>();
    }

    void Start()
    {
        RequestComponents();
    }

    void RequestComponents()
    {
        _requestedComponents = new();
        foreach (var component in _buildingComponents.GetRequiredAmounts())
        {
            var itemDef = component.Item1;
            var amount = component.Item2;
            _requestedComponents.Add(itemDef);
            Debug.Log($"Request item: {itemDef}, {amount}");
            OnItemRequest?.Invoke(itemDef, amount, _buildingComponents);
        }
    }

    public void FulfillComponentRequest(ItemDef itemDef)
    {
        _requestedComponents.Remove(itemDef);

        if (_requestedComponents.Count == 0)
            RequestConstruction();
    }

    void RequestConstruction()
    {
        Debug.Log($"RequestConstruction");
        OnConstructionRequest?.Invoke(_constructionWork);
    }

    public void FulfillConstructionRequest()
    {
        Debug.Log($"Construction finished");
    }
}