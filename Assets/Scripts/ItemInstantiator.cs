using UnityEngine;

public class ItemInstantiator : MonoBehaviour
{
    Grid2D _grid2D;

    void Awake()
    {
        _grid2D = transform.root.GetComponent<Grid2D>();
    }

    public GameObject Instantiate(Vector2Int cellPosition, ItemDef itemDef, ulong amount)
    {
        var position = _grid2D.CellCenterWorld(cellPosition);
        var gameObject = Instantiate(itemDef.Prefab, position, Quaternion.identity, transform.root);
        gameObject.GetComponent<ItemDefHolder>().Initialize(itemDef);
        gameObject.GetComponent<ItemAmount>().Initialize(amount);
        gameObject.SetActive(true);
        return gameObject;
    }
}
