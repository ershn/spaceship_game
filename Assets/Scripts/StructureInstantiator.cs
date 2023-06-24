using UnityEngine;

public class StructureInstantiator : MonoBehaviour
{
    public TaskScheduler TaskScheduler;
    public TilemapUpdater TilemapUpdater;
    public ItemCreator ItemCreator;

    public FloorDefHolder FloorPrefab;
    public FurnitureDefHolder FurniturePrefab;

    Grid2D _grid2D;

    void Awake()
    {
        _grid2D = transform.root.GetComponent<Grid2D>();
    }

    public GameObject InstantiateStructure(Vector2Int cellPosition, StructureDef structureDef)
    {
        var structure = structureDef switch
        {
            FloorDef def => InstantiateFloor(cellPosition, def),
            FurnitureDef def => InstantiateFurniture(cellPosition, def),
            _ => throw new System.NotImplementedException()
        };
        structure.SetActive(true);
        return structure;
    }

    T InstantiateAt<T>(Vector2Int cellPosition, T prefab)
        where T : Object
    {
        var position = _grid2D.CellCenterWorld(cellPosition);
        return Instantiate(prefab, position, Quaternion.identity, transform.root);
    }

    GameObject InstantiateFloor(Vector2Int cellPosition, FloorDef floorDef)
    {
        var floor = InstantiateAt(cellPosition, FloorPrefab);
        floor.FloorDef = floorDef;
        SetupCommonComponents(floor);
        return floor.gameObject;
    }

    GameObject InstantiateFurniture(Vector2Int cellPosition, FurnitureDef furnitureDef)
    {
        var furniture = InstantiateAt(cellPosition, FurniturePrefab);
        furniture.FurnitureDef = furnitureDef;
        SetupCommonComponents(furniture);
        return furniture.gameObject;
    }

    void SetupCommonComponents(StructureDefHolder structure)
    {
        structure.GetComponent<StructureComponents>().ItemCreator = ItemCreator;
        structure.GetComponent<StructureConstructor>().TaskScheduler = TaskScheduler;
        structure.GetComponent<StructureDeconstructor>().TaskScheduler = TaskScheduler;
        structure.GetComponent<StructureGraphics>().TilemapUpdater = TilemapUpdater;
    }
}
