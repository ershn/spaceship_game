using UnityEngine;

public class ItemInstantiator : MonoBehaviour
{
    [SerializeField]
    Transform _world;

    [SerializeField]
    Grid _grid;

    public GameObject Instantiate(Vector2Int cellPosition, ItemDef itemDef, ulong amount)
    {
        var position = _grid.GetCellCenterWorld((Vector3Int)cellPosition) + _world.position;
        var item = Instantiate(itemDef.Prefab, position, Quaternion.identity, _world);
        item.name = itemDef.name;
        item.GetComponent<ItemAmount>().Initialize(amount);
        item.SetActive(true);
        return item;
    }
}
