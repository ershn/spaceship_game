public class DeconstructionWork : TransactionalWork
{
    StructureDef _structureDef;

    protected override float RequiredTime =>
        _structureDef.ConstructionTime * _structureDef.DeconstructionTimeMultiplier;

    void Awake()
    {
        _structureDef = GetComponent<StructureDefHolder>().StructureDef;
    }
}
