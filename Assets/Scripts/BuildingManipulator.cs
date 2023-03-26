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

    public void CancelConstruction(Vector2Int cellPosition)
    {
        if (!FloorGrid.Has(cellPosition))
            return;

        Debug.Log($"CancelConstruction: {cellPosition}");
        var building = FloorGrid.Get(cellPosition);
        var constructor = building.GetComponent<BuildingConstructor>();
        constructor.Cancel();
    }
}