using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGridIndex : GOListGridIndex
{
    readonly Dictionary<ItemDef, HashSet<GameObject>> _itemDefIndex = new();
    readonly Dictionary<Type, HashSet<GameObject>> _itemDefTypeIndex = new();

    public ItemGridIndex(IGridIndex twinGrid = null)
        : base(twinGrid) { }

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

    public override void Add(Vector2Int position, GameObject gameObject)
    {
        base.Add(position, gameObject);

        var itemDef = gameObject.GetComponent<ItemDefHolder>().ItemDef;
        AddToIndex(_itemDefIndex, itemDef, gameObject);
        AddToIndex(_itemDefTypeIndex, itemDef.GetType(), gameObject);
    }

    public override void Remove(Vector2Int position, GameObject gameObject)
    {
        base.Remove(position, gameObject);

        var itemDef = gameObject.GetComponent<ItemDefHolder>().ItemDef;
        _itemDefIndex[itemDef].Remove(gameObject);
        _itemDefTypeIndex[itemDef.GetType()].Remove(gameObject);
    }

    void AddToIndex<TKey>(Dictionary<TKey, HashSet<GameObject>> index, TKey key, GameObject obj)
    {
        if (!index.TryGetValue(key, out var set))
        {
            set = new();
            index[key] = set;
        }
        set.Add(obj);
    }
}
