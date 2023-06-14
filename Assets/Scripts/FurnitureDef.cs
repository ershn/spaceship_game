using UnityEngine;

[CreateAssetMenu(menuName = "Structure/Furniture")]
public class FurnitureDef : StructureDef
{
    public override WorldLayer WorldLayer => WorldLayer.Furniture;
}
