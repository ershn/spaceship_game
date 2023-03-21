using UnityEngine;

public class BlueprintPlacer : MonoBehaviour
{
    public BuildingInstantiator BuildingInstantiator;
    public GridMonoIndexer TileGrid;

    public BuildingDef SteelFloor;

    public void PlaceBlueprint(Vector2Int cellPosition)
    {
        if (TileGrid.Has(cellPosition))
        {
            Debug.Log($"PlaceBlueprint: already a building at {cellPosition}");
            return;
        }

        Debug.Log($"PlaceBlueprint: {cellPosition}, {SteelFloor}");
        BuildingInstantiator.InstantiateFloor(cellPosition, SteelFloor);
    }
}