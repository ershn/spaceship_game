public abstract class AmountMode
{
    public abstract AmountType AmountType { get; }

    public abstract ulong AmountToMass(ulong amount);
}
