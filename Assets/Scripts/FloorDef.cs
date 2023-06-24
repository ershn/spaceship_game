using UnityEngine;

[CreateAssetMenu(menuName = "Structure/Floor")]
public class FloorDef : StructureDef
{
    public override WorldLayer WorldLayer => WorldLayer.Floor;

    public FloorCategory Category;

    public override bool IsConstructibleAt(Vector2Int cellPosition, GridIndexes gridIndexes) =>
        FloorConstructor.IsConstructibleAt(cellPosition, gridIndexes);
}
