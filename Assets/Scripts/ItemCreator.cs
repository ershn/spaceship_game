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
        var item = ItemGrid.Find(cellPosition, itemDef);
        if (item != null)
            item.GetComponent<ItemAmount>().Add(amount);
        else
            _instantiator.Instantiate(cellPosition, itemDef, amount);
    }
}