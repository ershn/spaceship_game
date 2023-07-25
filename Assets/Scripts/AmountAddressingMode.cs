public abstract class AmountAddressingMode
{
    public abstract AmountType AmountType { get; }

    public abstract ulong AmountToMass(ulong amount);
}
