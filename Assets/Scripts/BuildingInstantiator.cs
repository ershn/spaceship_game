using UnityEngine;

public class BuildingInstantiator : MonoBehaviour
{
    public ItemRequestManager ItemRequestManager;
    public ConstructionRequestManager ConstructionRequestManager;

    public BuildingDefHolder FloorPrefab;

    Grid2D _grid2D;

    void Awake()
    {
        _grid2D = transform.root.GetComponent<Grid2D>();
    }

    public BuildingDefHolder InstantiateFloor(Vector2Int cellPosition, BuildingDef buildingDef)
    {
        var position = _grid2D.CellCenterWorld(cellPosition);

        var floor = Instantiate(FloorPrefab, position, Quaternion.identity, transform.root);

        floor.BuildingDef = buildingDef;

        var constructionRequester = floor.GetComponent<ConstructionRequester>();
        constructionRequester.ItemRequestManager = ItemRequestManager;
        constructionRequester.ConstructionRequestManager = ConstructionRequestManager;

        floor.gameObject.SetActive(true);

        return floor;
    }
}