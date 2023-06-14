using UnityEngine;

public class StructureDefHolder : MonoBehaviour, IWorldLayerDef, IHealthDef
{
    public StructureDef StructureDef;

    public WorldLayer WorldLayer => StructureDef.WorldLayer;

    public int MaxHealthPoints => StructureDef.MaxHealthPoints;
}
