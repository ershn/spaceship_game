using UnityEngine;

[RequireComponent(typeof(ItemInstantiator))]
public class ItemCreator : MonoBehaviour
{
    ItemGridIndex _itemGrid;

    ItemInstantiator _instantiator;

    void Awake()
    {
        _itemGrid = transform.root.GetComponent<GridIndexes>().ItemGrid;

        _instantiator = GetComponent<ItemInstantiator>();
    }

    public void Upsert(Vector2Int cellPosition, ItemDef itemDef, ulong amount)
    {
        if (_itemGrid.TryGetItem(cellPosition, itemDef, out var item))
            item.GetComponent<ItemAmount>().Add(amount);
        else
            _instantiator.Instantiate(cellPosition, itemDef, amount);
    }
}
