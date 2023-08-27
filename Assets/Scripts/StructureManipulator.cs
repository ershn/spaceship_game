using System.Collections.Generic;
using UnityEngine;

public class StructureManipulator : MonoBehaviour
{
    [SerializeField]
    GridIndexes _gridIndexes;

    StructureInstantiator _structureInstantiator;

    void Awake()
    {
        _structureInstantiator = GetComponent<StructureInstantiator>();
    }

    public void Construct(Vector2Int cellPosition, StructureDef structureDef)
    {
        if (structureDef.IsConstructibleAt(_gridIndexes, cellPosition))
            _structureInstantiator.Instantiate(cellPosition, structureDef);
    }

    public void Deconstruct(Vector2Int cellPosition, WorldLayer structureLayers)
    {
        foreach (var structure in StructuresAt(cellPosition, structureLayers))
            structure.GetComponent<StructureDeconstructor>().Deconstruct();
    }

    public void Cancel(Vector2Int cellPosition, WorldLayer structureLayers)
    {
        foreach (var structure in StructuresAt(cellPosition, structureLayers))
            structure.GetComponent<Canceler>().Cancel();
    }

    IEnumerable<GameObject> StructuresAt(Vector2Int cellPosition, WorldLayer structureLayers)
    {
        var grids = _gridIndexes.GetStructureLayerIndexes(structureLayers);
        foreach (var grid in grids)
        {
            if (grid.TryGet(cellPosition, out var structure))
                yield return structure;
        }
    }
}
