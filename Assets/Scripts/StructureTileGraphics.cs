using UnityEngine;

[RequireComponent(typeof(GridPosition))]
[RequireComponent(typeof(StructureDefHolder))]
public class StructureTileGraphics : MonoBehaviour
{
    public TilemapUpdater TilemapUpdater;

    GridPosition _gridPosition;
    StructureDef _structureDef;

    void Awake()
    {
        _gridPosition = GetComponent<GridPosition>();
        _structureDef = GetComponent<StructureDefHolder>().StructureDef;
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
        TilemapUpdater.SetTile(_gridPosition.CellPosition, _structureDef.BlueprintTile);
    }

    public void ToNormalTile()
    {
        TilemapUpdater.SetTile(_gridPosition.CellPosition, _structureDef.NormalTile);
    }

    public void UnsetTile()
    {
        TilemapUpdater.UnsetTile(_gridPosition.CellPosition);
    }
}
