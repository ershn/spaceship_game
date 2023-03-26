using UnityEngine;

public class BuildingInstantiator : MonoBehaviour
{
    public TilemapUpdater TilemapUpdater;
    public ItemRequestManager ItemRequestManager;
    public ConstructionRequestManager ConstructionRequestManager;
    public ItemCreator ItemCreator;

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

        var constructor = floor.GetComponent<BuildingConstructor>();
        constructor.ItemRequestManager = ItemRequestManager;
        constructor.ConstructionRequestManager = ConstructionRequestManager;

        var components = floor.GetComponent<BuildingComponents>();
        components.ItemCreator = ItemCreator;

        var tileGraphics = floor.GetComponent<BuildingTileGraphics>();
        tileGraphics.TilemapUpdater = TilemapUpdater;

        floor.gameObject.SetActive(true);

        return floor;
    }
}