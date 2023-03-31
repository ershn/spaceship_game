public interface IAmountRemove : IComponent
{
    void Reserve(ulong amount);
    void Unreserve(ulong amount);
    void Remove(ulong amount);
}