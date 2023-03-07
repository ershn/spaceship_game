using UnityEngine;

public class BlueprintPlacer : MonoBehaviour
{
    public GridPositioner GridPositioner;

    public GameObject SteelFloor;

    public void PlaceBlueprint(Vector2Int cellPosition)
    {
        Debug.Log($"PlaceBlueprint: {cellPosition}, {SteelFloor}");
        var position = GridPositioner.CellCenterWorld(cellPosition);
        Instantiate(SteelFloor, position, Quaternion.identity);
    }
}