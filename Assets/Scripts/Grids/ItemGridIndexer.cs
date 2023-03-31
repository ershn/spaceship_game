using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid/Item")]
public class ItemGridIndexer : GridPolyIndexer
{
    Dictionary<ItemDef, Dictionary<Vector2Int, GameObject>> _index = new();

    public GameObject Find(Vector2Int position, ItemDef itemDef) =>
        Get(position).FirstOrDefault(item =>
            item.GetComponent<ItemDefHolder>().ItemDef == itemDef
            );

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