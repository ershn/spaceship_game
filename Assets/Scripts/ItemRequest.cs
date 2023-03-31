public class ItemRequest
{
    public ItemDef ItemDef { get; }
    public ulong Amount { get; }
    public IItemAmountAdd Inventory { get; }

    public ItemRequest(ItemDef itemDef, ulong amount, IItemAmountAdd inventory)
    {
        ItemDef = itemDef;
        Amount = amount;
        Inventory = inventory;
    }
}