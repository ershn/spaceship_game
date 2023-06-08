using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGridIndex : GOListGridIndex
{
    readonly Dictionary<ItemDef, HashSet<GameObject>> _itemDefIndex = new();
    readonly Dictionary<Type, HashSet<GameObject>> _itemDefTypeIndex = new();

    public bool TryGetItem(Vector2Int position, ItemDef itemDef, out GameObject obj)
    {
        obj = Get(position)
            .FirstOrDefault(item => item.GetComponent<ItemDefHolder>().ItemDef == itemDef);
        return obj != null;
    }

    public IEnumerable<GameObject> Filter(ItemDef itemDef) =>
        _itemDefIndex.TryGetValue(itemDef, out var list) ? list : new();

    public IEnumerable<GameObject> Filter<T>()
        where T : ItemDef => _itemDefTypeIndex.TryGetValue(typeof(T), out var list) ? list : new();

    protected override void OnAdd(GridPosition obj)
    {
        var itemDef = obj.GetComponent<ItemDefHolder>().ItemDef;
        AddToIndex(_itemDefIndex, itemDef, obj.gameObject);
        AddToIndex(_itemDefTypeIndex, itemDef.GetType(), obj.gameObject);
    }

    protected override void OnRemove(GridPosition obj)
    {
        var itemDef = obj.GetComponent<ItemDefHolder>().ItemDef;
        _itemDefIndex[itemDef].Remove(obj.gameObject);
        _itemDefTypeIndex[itemDef.GetType()].Remove(obj.gameObject);
    }

    private void AddToIndex<TKey>(
        Dictionary<TKey, HashSet<GameObject>> index,
        TKey key,
        GameObject obj
    )
    {
        if (!index.TryGetValue(key, out var set))
        {
            set = new();
            index[key] = set;
        }
        set.Add(obj);
    }
}
