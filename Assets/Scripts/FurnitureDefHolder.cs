public class FurnitureDefHolder : StructureDefHolder
{
    public override void Initialize(StructureDef structureDef)
    {
        FurnitureDef = (FurnitureDef)structureDef;
    }

    public override StructureDef StructureDef => FurnitureDef;

    public FurnitureDef FurnitureDef;
}
