using UnityEngine;

public class ItemInstantiator : MonoBehaviour
{
    public ItemDefHolder ItemPrefab;

    Grid2D _grid2D;

    void Awake()
    {
        _grid2D = transform.root.GetComponent<Grid2D>();
    }

    public ItemDefHolder Instantiate(Vector2Int cellPosition, ItemDef itemDef, ulong amount)
    {
        var position = _grid2D.CellCenterWorld(cellPosition);

        var item = Instantiate(ItemPrefab, position, Quaternion.identity, transform.root);

        item.ItemDef = itemDef;

        var itemAmount = item.GetComponent<ItemAmount>();
        itemAmount.Amount = amount;

        item.gameObject.SetActive(true);

        return item;
    }
}