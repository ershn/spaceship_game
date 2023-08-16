using UnityEngine;

public class StructureInstantiator : MonoBehaviour
{
    [SerializeField]
    Transform _world;

    [SerializeField]
    Grid2D _grid2D;

    public GameObject Instantiate(Vector2Int cellPosition, StructureDef structureDef)
    {
        var position = (Vector3)_grid2D.CellToWorld2D(cellPosition) + _world.position;
        var structure = Instantiate(structureDef.Prefab, position, Quaternion.identity, _world);
        Setup(structure, structureDef);
        return structure;
    }

    public static void Setup(GameObject structure, StructureDef structureDef)
    {
        structure.GetComponent<StructureDefHolder>().Initialize(structureDef);
        structure.SetActive(true);
    }
}
