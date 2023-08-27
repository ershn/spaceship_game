using UnityEngine;

[CustomGridBrush(
    hideAssetInstances: true,
    hideDefaultInstance: false,
    defaultBrush: false,
    defaultName: "Entity Brush"
)]
public class EntityBrush : GridBrushBase
{
    [SerializeField]
    EntityDef _entityDef;

    public override void Paint(GridLayout grid, GameObject tilemap, Vector3Int position)
    {
        var gridIndexes = grid.GetComponent<GridIndexes>();

        EntityPlacer.TryPlaceEntity(
            _entityDef,
            gridIndexes,
            (Vector2Int)position,
            tilemap.transform,
            ignorePlacementRules: IsTilePalette(tilemap)
        );
    }

    public override void Erase(GridLayout grid, GameObject _tilemap, Vector3Int position)
    {
        var gridIndexes = grid.GetComponent<GridIndexes>();

        EntityPlacer.RemoveEntities(gridIndexes, (Vector2Int)position);
    }

    public override void Pick(
        GridLayout grid,
        GameObject _tilemap,
        BoundsInt position,
        Vector3Int _pivot
    )
    {
        var gridIndexes = grid.GetComponent<GridIndexes>();

        if (EntityPlacer.TryPickEntity(gridIndexes, (Vector2Int)position.min, out var entityDef))
            _entityDef = entityDef;
    }

    bool IsTilePalette(GameObject tilemap) => !tilemap.TryGetComponent<World>(out _);
}
