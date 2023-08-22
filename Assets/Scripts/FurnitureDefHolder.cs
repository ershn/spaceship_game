public class FurnitureDefHolder : StructureDefHolder, ITemplate<StructureDef>
{
    public void Template(StructureDef structureDef)
    {
        FurnitureDef = (FurnitureDef)structureDef;
    }

    public override StructureDef StructureDef => FurnitureDef;

    public FurnitureDef FurnitureDef;
}
