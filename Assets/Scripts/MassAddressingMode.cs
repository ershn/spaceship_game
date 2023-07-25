using System;

[Serializable]
public class MassAddressingMode : AmountAddressingMode
{
    public override AmountType AmountType => AmountType.Mass;

    public override ulong AmountToMass(ulong mass) => mass;
}
