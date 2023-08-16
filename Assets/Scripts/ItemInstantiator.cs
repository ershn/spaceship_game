using UnityEngine;

public class ItemInstantiator : MonoBehaviour
{
    [SerializeField]
    Transform _world;

    [SerializeField]
    Grid2D _grid2D;

    public GameObject Instantiate(Vector2Int cellPosition, ItemDef itemDef, ulong amount)
    {
        var position = (Vector3)_grid2D.CellToWorld2D(cellPosition) + _world.position;
        var item = Instantiate(itemDef.Prefab, position, Quaternion.identity, _world);
        Setup(item, itemDef, amount);
        return item;
    }

    public static void Setup(GameObject item, ItemDef itemDef, ulong amount)
    {
        item.GetComponent<ItemDefHolder>().Initialize(itemDef);
        item.GetComponent<ItemAmount>().Initialize(amount);
        item.SetActive(true);
    }
}
