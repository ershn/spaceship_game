public class ItemDefHolder : EntityDefHolder, ITemplate<ItemDef>, IWorldLayerGet, IAmountModeGet
{
    public void Template(ItemDef itemDef)
    {
        ItemDef = itemDef;
    }

    public override EntityDef EntityDef => ItemDef;

    public ItemDef ItemDef;

    public WorldLayer WorldLayer => ItemDef.WorldLayer;

    // Used in editor
    public AmountMode AmountMode => ItemDef != null ? ItemDef.AmountMode : null;
}
