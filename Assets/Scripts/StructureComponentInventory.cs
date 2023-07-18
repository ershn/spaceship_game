public class StructureComponentInventory : StructureInventory
{
    protected override void Awake()
    {
        base.Awake();

        var structureDef = GetComponent<StructureDefHolder>().StructureDef;
        foreach (var component in structureDef.ComponentAmounts)
            AddSlot(component.ItemDef, component.Amount);
    }
}
