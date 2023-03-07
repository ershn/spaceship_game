using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridPlacer))]
[RequireComponent(typeof(BuildingDefHolder))]
public class BuildingTileGraphics : MonoBehaviour
{
    public Vector2IntTileBaseGameEvent OnTileChanged;

    GridPlacer _gridPlacer;
    BuildingDef _buildingDef;

    void Awake()
    {
        _gridPlacer = GetComponent<GridPlacer>();
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
