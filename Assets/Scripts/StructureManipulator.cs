using UnityEngine;

public class StructureManipulator : MonoBehaviour
{
    GOGridIndex _floorGrid;

    StructureInstantiator _structureInstantiator;

    void Awake()
    {
        _floorGrid = transform.root.GetComponent<GridIndexes>().FloorGrid;

        _structureInstantiator = GetComponent<StructureInstantiator>();
    }

    public void Construct(Vector2Int cellPosition, StructureDef structureDef)
    {
        if (_floorGrid.Has(cellPosition))
            return;

        _structureInstantiator.InstantiateFloor(cellPosition, structureDef);
    }

    public void Deconstruct(Vector2Int cellPosition)
    {
        if (!_floorGrid.TryGet(cellPosition, out var structure))
            return;

        structure.GetComponent<StructureDeconstructor>().Deconstruct();
    }

    public void Cancel(Vector2Int cellPosition)
    {
        if (!_floorGrid.TryGet(cellPosition, out var structure))
            return;

        if (structure.TryGetComponent<StructureConstructor>(out var constructor))
            constructor.Cancel();
        else
            structure.GetComponent<StructureDeconstructor>().Cancel();
    }
}
