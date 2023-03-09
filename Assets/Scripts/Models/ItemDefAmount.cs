using System;

[Serializable]
public struct ItemDefAmount
{
    public ItemDef ItemDef;
    public ulong Amount;

    public ItemDefAmount(ItemDef itemDef, ulong amount)
    {
        ItemDef = itemDef;
        Amount = amount;
    }
}
