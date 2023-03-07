using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ItemGridIndexer : GridIndexer
{
    [Serializable]
    public class ItemEvent : UnityEvent<Vector2Int, GameObject, ItemDef> {}

    public ItemEvent OnItemAdded;
    public ItemEvent OnItemRemoved;

    Dictionary<ItemDef, Dictionary<Vector2Int, GameObject>> _index = new();

    public IEnumerable<GameObject> GetAllItems(ItemDef itemDef)
    {
        return (_index.TryGetValue(itemDef, out var dict) ? dict : new()).Values;
    }

    protected override void OnAdd(Vector2Int position, GameObject gameObject)
    {
        var itemDef = gameObject.GetComponent<ItemDefHolder>().ItemDef;
        if (!_index.TryGetValue(itemDef, out var itemIndex))
        {
            itemIndex = new();
            _index[itemDef] = itemIndex;
        }
        itemIndex[position] = gameObject;

        OnItemAdded.Invoke(position, gameObject, itemDef);
    }

    protected override void OnRemove(Vector2Int position, GameObject gameObject)
    {
        var itemDef = gameObject.GetComponent<ItemDefHolder>().ItemDef;
        _index[itemDef].Remove(position);

        OnItemRemoved.Invoke(position, gameObject, itemDef);
    }
}