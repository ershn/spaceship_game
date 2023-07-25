using System;

[Serializable]
public struct ItemDefAmount : IAmountHolderConf
{
    public ItemDef ItemDef;

    [Amount]
    public ulong Amount;

    public readonly AmountAddressingMode AmountAddressingMode => ItemDef.AmountAddressingMode;
}
