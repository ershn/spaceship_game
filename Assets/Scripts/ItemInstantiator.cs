using UnityEngine;

public class ItemInstantiator : MonoBehaviour
{
    public Grid2D Grid2D;

    public ItemDefHolder ItemPrefab;

    public ItemDefHolder Instantiate(Vector2Int cellPosition, ItemDef itemDef, ulong amount)
    {
        var position = Grid2D.CellCenterWorld(cellPosition);

        var item = Instantiate(ItemPrefab, position, Quaternion.identity);

        item.ItemDef = itemDef;

        var gridPosition = item.GetComponent<GridPosition>();
        gridPosition.Grid2D = Grid2D;

        var itemAmount = item.GetComponent<ItemAmount>();
        itemAmount.Amount = amount;

        item.gameObject.SetActive(true);

        return item;
    }
}