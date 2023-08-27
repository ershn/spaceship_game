public abstract class StructureDefHolder : EntityDefHolder, IWorldLayerGet, IHealthHolderConf
{
    public override EntityDef EntityDef => StructureDef;

    public abstract StructureDef StructureDef { get; }

    public WorldLayer WorldLayer => StructureDef.WorldLayer;

    public int MaxHealthPoints => StructureDef.MaxHealthPoints;
}
