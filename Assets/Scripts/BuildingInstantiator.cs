using UnityEngine;

public class BuildingInstantiator : MonoBehaviour
{
    public Grid2D Grid2D;
    public ItemRequestManager ItemRequestManager;
    public ConstructionRequestManager ConstructionRequestManager;

    public BuildingDefHolder FloorPrefab;

    public BuildingDefHolder InstantiateFloor(Vector2Int cellPosition, BuildingDef buildingDef)
    {
        var position = Grid2D.CellCenterWorld(cellPosition);

        var floor = Instantiate(FloorPrefab, position, Quaternion.identity);

        floor.BuildingDef = buildingDef;

        var gridPosition = floor.GetComponent<GridPosition>();
        gridPosition.Grid2D = Grid2D;

        var constructionRequester = floor.GetComponent<ConstructionRequester>();
        constructionRequester.ItemRequestManager = ItemRequestManager;
        constructionRequester.ConstructionRequestManager = ConstructionRequestManager;

        floor.gameObject.SetActive(true);

        return floor;
    }
}