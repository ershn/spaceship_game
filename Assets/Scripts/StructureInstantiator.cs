using UnityEngine;

public class StructureInstantiator : MonoBehaviour
{
    Grid2D _grid2D;

    void Awake()
    {
        _grid2D = transform.root.GetComponent<Grid2D>();
    }

    public GameObject Instantiate(Vector2Int cellPosition, StructureDef structureDef)
    {
        var position = _grid2D.CellCenterWorld(cellPosition);
        var gameObject = Instantiate(
            structureDef.Prefab,
            position,
            Quaternion.identity,
            transform.root
        );
        gameObject.GetComponent<StructureDefHolder>().Initialize(structureDef);
        gameObject.SetActive(true);
        return gameObject;
    }
}
