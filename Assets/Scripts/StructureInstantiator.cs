using UnityEngine;

public class StructureInstantiator : MonoBehaviour
{
    public TaskScheduler TaskScheduler;
    public TilemapUpdater TilemapUpdater;
    public ItemCreator ItemCreator;

    public ItemGridIndexer ItemGrid;

    public StructureDefHolder StructurePrefab;

    Grid2D _grid2D;

    void Awake()
    {
        _grid2D = transform.root.GetComponent<Grid2D>();
    }

    public StructureDefHolder InstantiateFloor(Vector2Int cellPosition, StructureDef structureDef)
    {
        var position = _grid2D.CellCenterWorld(cellPosition);

        var floor = Instantiate(StructurePrefab, position, Quaternion.identity, transform.root);

        floor.StructureDef = structureDef;

        var components = floor.GetComponent<StructureComponents>();
        components.ItemCreator = ItemCreator;

        var constructor = floor.GetComponent<StructureConstructor>();
        constructor.ItemGrid = ItemGrid;
        constructor.TaskScheduler = TaskScheduler;

        var deconstructor = floor.GetComponent<StructureDeconstructor>();
        deconstructor.TaskScheduler = TaskScheduler;

        var tileGraphics = floor.GetComponent<StructureTileGraphics>();
        tileGraphics.TilemapUpdater = TilemapUpdater;

        floor.gameObject.SetActive(true);

        return floor;
    }
}
