using UnityEngine;

[CreateAssetMenu(menuName = "Structure/Furniture")]
public class FurnitureDef : StructureDef
{
    public override WorldLayer WorldLayer => WorldLayer.Furniture;

    public FloorCategory PlaceableFloorCategory;

    public override bool IsConstructibleAt(Vector2Int cellPosition, GridIndexes gridIndexes) =>
        FurnitureConstructor.IsConstructibleAt(this, cellPosition, gridIndexes);
}
