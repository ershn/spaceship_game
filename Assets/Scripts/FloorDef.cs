using UnityEngine;

[CreateAssetMenu(menuName = "Structure/Floor")]
public class FloorDef : StructureDef
{
    public override WorldLayer WorldLayer => WorldLayer.Floor;

    [Header("Construction restrictions")]
    public FloorCategory Category;

    public override bool IsConstructibleAt(
        GridIndexes gridIndexes,
        Vector2Int cellPosition,
        bool ignoreExisting = false
    )
    {
        if (!ignoreExisting && gridIndexes.FloorGrid.Has(cellPosition))
            return false;
        return true;
    }
}
