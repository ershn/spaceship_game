public interface IAmount : IAmountAdd, IAmountRemove
{
    ulong Get();
    void Reserve(ulong amount);
}