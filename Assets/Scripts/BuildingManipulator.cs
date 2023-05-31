using UnityEngine;

public class BuildingManipulator : MonoBehaviour
{
    public GridMonoIndexer FloorGrid;

    BuildingInstantiator _buildingInstantiator;

    void Awake()
    {
        _buildingInstantiator = GetComponent<BuildingInstantiator>();
    }

    public void Construct(Vector2Int cellPosition, BuildingDef buildingDef)
    {
        if (FloorGrid.Has(cellPosition))
            return;

        Debug.Log($"Construct: {cellPosition}, {buildingDef}");
        _buildingInstantiator.InstantiateFloor(cellPosition, buildingDef);
    }

    public void Deconstruct(Vector2Int cellPosition)
    {
        if (!FloorGrid.TryGet(cellPosition, out var building))
            return;
        Debug.Log($"Deconstruct: {cellPosition}");
        building.GetComponent<BuildingDeconstructor>().Deconstruct();
    }

    public void Cancel(Vector2Int cellPosition)
    {
        if (!FloorGrid.TryGet(cellPosition, out var building))
            return;
        Debug.Log($"Cancel: {cellPosition}");
        if (building.TryGetComponent<BuildingConstructor>(out var constructor))
            constructor.Cancel();
        else
            building.GetComponent<BuildingDeconstructor>().Cancel();
    }
}
