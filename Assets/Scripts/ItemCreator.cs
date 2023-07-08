using System;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public event Action<ItemAmount> OnItemCreated;

    ItemGridIndex _itemGrid;

    ItemInstantiator _instantiator;

    void Awake()
    {
        _itemGrid = transform.root.GetComponent<GridIndexes>().ItemGrid;

        _instantiator = GetComponent<ItemInstantiator>();
    }

    public void Create(Vector2Int cellPosition, ItemDef itemDef, ulong amount)
    {
        ItemAmount itemAmount;

        if (_itemGrid.TryGetItem(cellPosition, itemDef, out var item))
        {
            itemAmount = item.GetComponent<ItemAmount>();
            itemAmount.Add(amount);
        }
        else
        {
            item = _instantiator.Instantiate(cellPosition, itemDef, amount);
            itemAmount = item.GetComponent<ItemAmount>();
        }

        OnItemCreated?.Invoke(itemAmount);
    }
}
