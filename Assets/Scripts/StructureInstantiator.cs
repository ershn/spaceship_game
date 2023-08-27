using UnityEngine;

public class StructureInstantiator : MonoBehaviour
{
    [SerializeField]
    Transform _world;

    [SerializeField]
    Grid _grid;

    public GameObject Instantiate(Vector2Int cellPosition, StructureDef structureDef)
    {
        var position = _grid.GetCellCenterWorld((Vector3Int)cellPosition) + _world.position;
        var structure = Instantiate(structureDef.Prefab, position, Quaternion.identity, _world);
        structure.name = structureDef.name;
        structure.SetActive(true);
        return structure;
    }
}
