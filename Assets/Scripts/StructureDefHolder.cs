using UnityEngine;

public abstract class StructureDefHolder : MonoBehaviour, IWorldLayerGet, IHealthHolderConf
{
    public abstract StructureDef StructureDef { get; }

    public WorldLayer WorldLayer => StructureDef.WorldLayer;

    public int MaxHealthPoints => StructureDef.MaxHealthPoints;
}
