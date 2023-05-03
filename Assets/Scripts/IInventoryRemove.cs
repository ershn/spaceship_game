public interface IInventoryRemove : IComponent
{
    void Remove(ItemDef itemDef, ulong mass);
}