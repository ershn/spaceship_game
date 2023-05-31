public interface IInventoryAdd : IComponent
{
    void Add(ItemDef itemDef, ulong amount);
}
