using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemGridIndexer : GridPolyIndexer
{
    // public delegate void ItemEvent(Vector2Int position, GameObject item, ItemDef itemDef);

    // public event ItemEvent OnItemAdded;
    // public event ItemEvent OnItemRemoved;

    Dictionary<ItemDef, Dictionary<Vector2Int, GameObject>> _index = new();

    public IEnumerable<GameObject> GetAllItems(ItemDef itemDef)
    {
        return (_index.TryGetValue(itemDef, out var dict) ? dict : new()).Values;
    }

    protected override void OnAdd(GridPosition obj)
    {
        var itemDef = obj.GetComponent<ItemDefHolder>().ItemDef;
        if (!_index.TryGetValue(itemDef, out var itemIndex))
        {
            itemIndex = new();
            _index[itemDef] = itemIndex;
        }
        itemIndex[obj.CellPosition] = obj.gameObject;
    }

    protected override void OnRemove(GridPosition obj)
    {
        var itemDef = obj.GetComponent<ItemDefHolder>().ItemDef;
        _index[itemDef].Remove(obj.CellPosition);
    }
}