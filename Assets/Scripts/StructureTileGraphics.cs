using UnityEngine;

public class StructureTileGraphics : MonoBehaviour
{
    StructureTileGraphicsDef _tileGraphicsDef;

    TilemapUpdater _tilemapUpdater;
    GridPosition _gridPosition;
    StructureGraphics _structureGraphics;

    void Awake()
    {
        var structureDef = GetComponent<StructureDefHolder>().StructureDef;
        _tileGraphicsDef = (StructureTileGraphicsDef)structureDef.StructureGraphicsDef;

        _tilemapUpdater = GetComponentInParent<WorldInternalIO>().TilemapUpdater;
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
        _tilemapUpdater.SetTile(_gridPosition.CellPosition, _tileGraphicsDef.BlueprintTile);
    }

    void ToNormalGraphics()
    {
        _tilemapUpdater.SetTile(_gridPosition.CellPosition, _tileGraphicsDef.NormalTile);
    }

    void UnsetTile()
    {
        _tilemapUpdater.UnsetTile(_gridPosition.CellPosition);
    }
}
