using UnityEngine;

[CreateAssetMenu(menuName = "Structure/Furniture")]
public class FurnitureDef : StructureDef
{
    public override WorldLayer WorldLayer => WorldLayer.Furniture;

    [Header("Construction restrictions")]
    public FloorCategory PlaceableFloorCategory;

    public override bool IsConstructibleAt(
        GridIndexes gridIndexes,
        Vector2Int cellPosition,
        bool ignoreExisting = false
    )
    {
        if (!ignoreExisting && gridIndexes.FurnitureGrid.Has(cellPosition))
            return false;
        if (!gridIndexes.FloorGrid.TryGet(cellPosition, out var floor))
            return false;
        var floorDef = floor.GetComponent<FloorDefHolder>().FloorDef;
        return floorDef.Category == PlaceableFloorCategory;
    }
}
