using System;

[Serializable]
public struct ItemDefAmount : IAmountModeGet
{
    public ItemDef ItemDef;

    [Amount]
    public ulong Amount;

    public readonly AmountMode AmountMode => ItemDef.AmountMode;
}
