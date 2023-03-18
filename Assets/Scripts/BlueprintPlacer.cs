using UnityEngine;

public class BlueprintPlacer : MonoBehaviour
{
    public GridPositioner GridPositioner;

    public BuildingDefHolder Floor;
    public BuildingDef SteelFloor;

    public void PlaceBlueprint(Vector2Int cellPosition)
    {
        Debug.Log($"PlaceBlueprint: {cellPosition}, {Floor}");
        var position = GridPositioner.CellCenterWorld(cellPosition);
        var floor = Instantiate(Floor, position, Quaternion.identity);
        floor.BuildingDef = SteelFloor;
        floor.gameObject.SetActive(true);
    }
}