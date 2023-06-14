using UnityEngine;

[CreateAssetMenu(menuName = "Structure/Floor")]
public class FloorDef : StructureDef
{
    public override WorldLayer WorldLayer => WorldLayer.Floor;

    public bool Cultivable;
}
