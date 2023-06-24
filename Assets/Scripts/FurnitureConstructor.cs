using System;
using UnityEngine;

public class FurnitureConstructor : MonoBehaviour
{
    public static bool IsConstructibleAt(
        FurnitureDef furnitureDef,
        Vector2Int cellPosition,
        GridIndexes gridIndexes
    )
    {
        if (gridIndexes.FurnitureGrid.Has(cellPosition))
            return false;
        if (!gridIndexes.FloorGrid.TryGet(cellPosition, out var floor))
            return false;
        var floorDef = (FloorDef)floor.GetComponent<StructureDefHolder>().StructureDef;
        return floorDef.Category == furnitureDef.PlaceableFloorCategory;
    }

    Action _cleanup;

    void Start()
    {
        var floorGrid = transform.root.GetComponent<GridIndexes>().FloorGrid;
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
