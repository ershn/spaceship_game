using UnityEngine;

[RequireComponent(typeof(GridPosition))]
[RequireComponent(typeof(BuildingDefHolder))]
public class BuildingTileGraphics : MonoBehaviour
{
    public Vector2IntTileBaseGameEvent OnTileChanged;

    GridPosition _gridPlacer;
    BuildingDef _buildingDef;

    void Awake()
    {
        _gridPlacer = GetComponent<GridPosition>();
        _buildingDef = GetComponent<BuildingDefHolder>().BuildingDef;
    }

    void Start()
    {
        ToBlueprintTile();
    }

    public void ToBlueprintTile()
    {
        OnTileChanged.Invoke(_gridPlacer.CellPosition, _buildingDef.BlueprintTile);
    }

    public void ToNormalTile()
    {
        OnTileChanged.Invoke(_gridPlacer.CellPosition, _buildingDef.NormalTile);
    }
}
