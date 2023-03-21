using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
    public GridPositioner GridPositioner;

    public ItemRequestManager ItemRequestManager;
    public ConstructionRequestManager ConstructionRequestManager;

    public BuildingDefHolder FloorPrefab;

    public BuildingDefHolder CreateFloor(Vector2Int cellPosition, BuildingDef buildingDef)
    {
        var position = GridPositioner.CellCenterWorld(cellPosition);

        var floor = Instantiate(FloorPrefab, position, Quaternion.identity);

        floor.BuildingDef = buildingDef;

        var requester = floor.GetComponent<ConstructionRequester>();
        requester.ItemRequestManager = ItemRequestManager;
        requester.ConstructionRequestManager = ConstructionRequestManager;

        floor.gameObject.SetActive(true);

        return floor;
    }
}