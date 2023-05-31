public class MassAddressingMode : AmountAddressingMode
{
    public override ulong AmountToMass(ulong mass) => mass;
}
