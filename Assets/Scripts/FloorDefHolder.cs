public class FloorDefHolder : StructureDefHolder, ITemplate<StructureDef>
{
    public void Template(StructureDef structureDef)
    {
        FloorDef = (FloorDef)structureDef;
    }

    public override StructureDef StructureDef => FloorDef;

    public FloorDef FloorDef;
}
