using UnityEngine;

[RequireComponent(typeof(GridPosition))]
[RequireComponent(typeof(BuildingDefHolder))]
public class BuildingTileGraphics : MonoBehaviour
{
    public TilemapUpdater TilemapUpdater;

    GridPosition _gridPosition;
    BuildingDef _buildingDef;

    void Awake()
    {
        _gridPosition = GetComponent<GridPosition>();
        _buildingDef = GetComponent<BuildingDefHolder>().BuildingDef;
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
        TilemapUpdater.SetTile(_gridPosition.CellPosition, _buildingDef.BlueprintTile);
    }

    public void ToNormalTile()
    {
        TilemapUpdater.SetTile(_gridPosition.CellPosition, _buildingDef.NormalTile);
    }

    public void UnsetTile()
    {
        TilemapUpdater.UnsetTile(_gridPosition.CellPosition);
    }
}
