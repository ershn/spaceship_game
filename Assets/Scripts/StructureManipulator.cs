using UnityEngine;

public class StructureManipulator : MonoBehaviour
{
    public GridMonoIndexer FloorGrid;

    StructureInstantiator _structureInstantiator;

    void Awake()
    {
        _structureInstantiator = GetComponent<StructureInstantiator>();
    }

    public void Construct(Vector2Int cellPosition, StructureDef structureDef)
    {
        if (FloorGrid.Has(cellPosition))
            return;

        Debug.Log($"Construct: {cellPosition}, {structureDef}");
        _structureInstantiator.InstantiateFloor(cellPosition, structureDef);
    }

    public void Deconstruct(Vector2Int cellPosition)
    {
        if (!FloorGrid.TryGet(cellPosition, out var structure))
            return;
        Debug.Log($"Deconstruct: {cellPosition}");
        structure.GetComponent<StructureDeconstructor>().Deconstruct();
    }

    public void Cancel(Vector2Int cellPosition)
    {
        if (!FloorGrid.TryGet(cellPosition, out var structure))
            return;
        Debug.Log($"Cancel: {cellPosition}");
        if (structure.TryGetComponent<StructureConstructor>(out var constructor))
            constructor.Cancel();
        else
            structure.GetComponent<StructureDeconstructor>().Cancel();
    }
}
