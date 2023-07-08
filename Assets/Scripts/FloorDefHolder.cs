public class FloorDefHolder : StructureDefHolder
{
    public override void Initialize(StructureDef structureDef)
    {
        FloorDef = (FloorDef)structureDef;
    }

    public override StructureDef StructureDef => FloorDef;

    public FloorDef FloorDef;
}
