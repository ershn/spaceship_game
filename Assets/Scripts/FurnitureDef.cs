using UnityEngine;

[CreateAssetMenu(menuName = "Structure/Furniture")]
public class FurnitureDef : StructureDef
{
    public override GridIndexType GridIndexType => GridIndexType.FurnitureGrid;
}
