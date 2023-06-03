using UnityEngine;

[RequireComponent(typeof(GridPosition))]
[RequireComponent(typeof(StructureDefHolder))]
public class StructureTileGraphics : MonoBehaviour
{
    public TilemapUpdater TilemapUpdater;

    GridPosition _gridPosition;
    StructureTileGraphicsDef _tileGraphicsDef;

    void Awake()
    {
        _gridPosition = GetComponent<GridPosition>();

        var structureDef = GetComponent<StructureDefHolder>().StructureDef;
        _tileGraphicsDef = (StructureTileGraphicsDef)structureDef.StructureGraphicsDef;
    }

    void Start()
    {
        ToBlueprintTile();
    }

    void OnDestroy()
    {
        UnsetTile();
    }

    public void ToBlueprintTile()
    {
        TilemapUpdater.SetTile(_gridPosition.CellPosition, _tileGraphicsDef.BlueprintTile);
    }

    public void ToNormalTile()
    {
        TilemapUpdater.SetTile(_gridPosition.CellPosition, _tileGraphicsDef.NormalTile);
    }

    public void UnsetTile()
    {
        TilemapUpdater.UnsetTile(_gridPosition.CellPosition);
    }
}
