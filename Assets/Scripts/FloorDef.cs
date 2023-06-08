using UnityEngine;

[CreateAssetMenu(menuName = "Structure/Floor")]
public class FloorDef : StructureDef
{
    public override GridIndexType GridIndexType => GridIndexType.FloorGrid;

    public bool Cultivable;
}
