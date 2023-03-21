using UnityEngine;

public class BlueprintPlacer : MonoBehaviour
{
    public BuildingCreator BuildingCreator;

    public BuildingDef SteelFloor;

    public void PlaceBlueprint(Vector2Int cellPosition)
    {
        Debug.Log($"PlaceBlueprint: {cellPosition}, {SteelFloor}");
        BuildingCreator.CreateFloor(cellPosition, SteelFloor);
    }
}