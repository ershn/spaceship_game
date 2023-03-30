using UnityEngine;

public class BuildingInstantiator : MonoBehaviour
{
    public TilemapUpdater TilemapUpdater;
    public ItemRequestManager ItemRequestManager;
    public WorkRequestManager WorkRequestManager;
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

        var components = floor.GetComponent<BuildingComponents>();
        components.ItemCreator = ItemCreator;

        var constructor = floor.GetComponent<BuildingConstructor>();
        constructor.ItemRequestManager = ItemRequestManager;
        constructor.WorkRequestManager = WorkRequestManager;

        var deconstructor = floor.GetComponent<BuildingDeconstructor>();
        deconstructor.WorkRequestManager = WorkRequestManager;

        var tileGraphics = floor.GetComponent<BuildingTileGraphics>();
        tileGraphics.TilemapUpdater = TilemapUpdater;

        floor.gameObject.SetActive(true);

        return floor;
    }
}