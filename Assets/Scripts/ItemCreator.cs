using System;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public event Action<ItemAmount> OnItemCreated;

    [SerializeField]
    GridIndexes _gridIndexes;

    ItemInstantiator _instantiator;

    void Awake()
    {
        _instantiator = GetComponent<ItemInstantiator>();
    }

    ItemGridIndex ItemGrid => _gridIndexes.ItemGrid;

    public void Create(Vector2Int cellPosition, ItemDef itemDef, ulong amount)
    {
        ItemAmount itemAmount;

        if (ItemGrid.TryGetItem(cellPosition, itemDef, out var item))
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
