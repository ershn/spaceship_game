public class StructureComponentInventory : StructureInventory
{
    protected override void Awake()
    {
        base.Awake();

        if (!Setup)
        {
            var structureDef = GetComponent<StructureDefHolder>().StructureDef;
            foreach (var component in structureDef.ComponentAmounts)
                AddSlot(component.ItemDef, component.Amount);
        }
    }
}
