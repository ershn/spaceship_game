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
        var gameObject = Instantiate(itemDef.Prefab, position, Quaternion.identity, _world);
        gameObject.GetComponent<ItemDefHolder>().Initialize(itemDef);
        gameObject.GetComponent<ItemAmount>().Initialize(amount);
        gameObject.SetActive(true);
        return gameObject;
    }
}
