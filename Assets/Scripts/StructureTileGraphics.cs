using UnityEngine;

[RequireComponent(typeof(GridPosition))]
public class StructureTileGraphics : MonoBehaviour, IStructureGraphics
{
    TilemapUpdater _tilemapUpdater;
    GridPosition _gridPosition;
    StructureTileGraphicsDef _tileGraphicsDef;

    void Awake()
    {
        _tilemapUpdater = transform.root.GetComponent<WorldInternalIO>().TilemapUpdater;
        _gridPosition = GetComponent<GridPosition>();

        var structureDef = GetComponent<StructureDefHolder>().StructureDef;
        _tileGraphicsDef = (StructureTileGraphicsDef)structureDef.StructureGraphicsDef;
    }

    void Start()
    {
        ToBlueprintGraphics();
    }

    void OnDestroy()
    {
        UnsetTile();
    }

    public void ToBlueprintGraphics()
    {
        _tilemapUpdater.SetTile(_gridPosition.CellPosition, _tileGraphicsDef.BlueprintTile);
    }

    public void ToNormalGraphics()
    {
        _tilemapUpdater.SetTile(_gridPosition.CellPosition, _tileGraphicsDef.NormalTile);
    }

    void UnsetTile()
    {
        _tilemapUpdater.UnsetTile(_gridPosition.CellPosition);
    }
}
