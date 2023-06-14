using System.Collections.Generic;
using UnityEngine;

public class StructureManipulator : MonoBehaviour
{
    GridIndexes _gridIndexes;
    StructureInstantiator _structureInstantiator;

    void Awake()
    {
        _gridIndexes = transform.root.GetComponent<GridIndexes>();
        _structureInstantiator = GetComponent<StructureInstantiator>();
    }

    public void Construct(Vector2Int cellPosition, StructureDef structureDef)
    {
        var grid = _gridIndexes.GetStructureLayerIndex(structureDef.WorldLayer);
        if (grid.Has(cellPosition))
            return;

        _structureInstantiator.InstantiateFloor(cellPosition, structureDef);
    }

    public void Deconstruct(Vector2Int cellPosition, WorldLayer structureLayers)
    {
        foreach (var structure in StructuresAt(cellPosition, structureLayers))
            structure.GetComponent<StructureDeconstructor>().Deconstruct();
    }

    public void Cancel(Vector2Int cellPosition, WorldLayer structureLayers)
    {
        foreach (var structure in StructuresAt(cellPosition, structureLayers))
            structure.GetComponent<StructureCanceler>().Cancel();
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
