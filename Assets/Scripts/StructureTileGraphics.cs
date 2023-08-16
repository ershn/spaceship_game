using UnityEngine;

public class StructureTileGraphics : MonoBehaviour
{
    StructureTileGraphicsDef _tileGraphicsDef;

    TilemapController _tilemapController;
    GridPosition _gridPosition;
    StructureGraphics _structureGraphics;

    void Awake()
    {
        var structureDef = GetComponent<StructureDefHolder>().StructureDef;
        _tileGraphicsDef = (StructureTileGraphicsDef)structureDef.StructureGraphicsDef;

        _tilemapController = GetComponentInParent<WorldInternalIO>().TilemapController;
        _gridPosition = GetComponent<GridPosition>();
        _structureGraphics = GetComponent<StructureGraphics>();

        Setup();
    }

    void Setup()
    {
        _structureGraphics.OnConstructionCompleted += OnConstructionCompleted;
    }

    void Start()
    {
        ToBlueprintGraphics();
    }

    void OnDestroy()
    {
        UnsetTile();
    }

    void OnConstructionCompleted() => ToNormalGraphics();

    void ToBlueprintGraphics()
    {
        _tilemapController.SetTile(
            _gridPosition.CellPosition,
            _tileGraphicsDef.Tile,
            _tileGraphicsDef.BlueprintColor
        );
    }

    void ToNormalGraphics()
    {
        _tilemapController.SetTile(_gridPosition.CellPosition, _tileGraphicsDef.Tile, Color.white);
    }

    void UnsetTile()
    {
        _tilemapController.UnsetTile(_gridPosition.CellPosition);
    }
}
