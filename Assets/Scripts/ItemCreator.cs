using UnityEngine;

[RequireComponent(typeof(ItemInstantiator))]
public class ItemCreator : MonoBehaviour
{
    public ItemGridIndexer ItemGrid;

    ItemInstantiator _instantiator;

    void Awake()
    {
        _instantiator = GetComponent<ItemInstantiator>();
    }

    public void Upsert(Vector2Int cellPosition, ItemDef itemDef, ulong amount)
    {
        if (ItemGrid.TryGetItem(cellPosition, itemDef, out var item))
            item.GetComponent<ItemAmount>().Add(amount);
        else
            _instantiator.Instantiate(cellPosition, itemDef, amount);
    }
}
