using UnityEngine;

public abstract class StructureDefHolder : MonoBehaviour, IWorldLayerMemberConf, IHealthHolderConf
{
    public abstract void Initialize(StructureDef structureDef);

    public abstract StructureDef StructureDef { get; }

    public WorldLayer WorldLayer => StructureDef.WorldLayer;

    public int MaxHealthPoints => StructureDef.MaxHealthPoints;

    void Awake()
    {
        name = StructureDef.name;
    }
}
