using System;
using UnityEngine;

public class FurnitureConstructor : MonoBehaviour
{
    Action _cleanup;

    void Start()
    {
        var floorGrid = GetComponentInParent<GridIndexes>().FloorGrid;
        var floor = floorGrid.Get(GetComponent<GridPosition>().CellPosition);

        floor.GetComponent<Destructor>().OnDestruction.AddListener(Destroy);

        if (floor.TryGetComponent<StructureConstructor>(out var floorConstructor))
        {
            _cleanup += floorConstructor.OnConstructionCompleted.Register(Construct);
            _cleanup += GetComponent<Canceler>().OnCancel.Register(Destroy);
        }
        else
            Construct();
    }

    void Construct()
    {
        _cleanup?.Invoke();
        GetComponent<StructureConstructor>().Construct();
    }

    void Destroy()
    {
        _cleanup?.Invoke();
        GetComponent<Canceler>().Cancel();
        GetComponent<Destructor>().Destroy();
    }
}
