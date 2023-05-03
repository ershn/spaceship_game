using System;

[Serializable]
public struct ItemDefMass
{
    public ItemDef ItemDef;
    public ulong Mass;

    public ItemDefMass(ItemDef itemDef, ulong mass)
    {
        ItemDef = itemDef;
        Mass = mass;
    }
}
