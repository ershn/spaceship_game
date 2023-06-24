using UnityEngine;

[RequireComponent(typeof(GridPosition))]
public class StructureTileGraphics : MonoBehaviour, IStructureGraphics
{
    GridPosition _gridPosition;
    StructureTileGraphicsDef _tileGraphicsDef;
    TilemapUpdater _tilemapUpdater;

    void Awake()
    {
        _gridPosition = GetComponent<GridPosition>();

        var structureDef = GetComponent<StructureDefHolder>().StructureDef;
        _tileGraphicsDef = (StructureTileGraphicsDef)structureDef.StructureGraphicsDef;
    }

    public void Setup(TilemapUpdater tilemapUpdater)
    {
        _tilemapUpdater = tilemapUpdater;
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
