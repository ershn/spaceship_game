using UnityEngine;

public class GridPosition : MonoBehaviour
{
    GridIndex _gridIndex;

    Grid2D _grid2D;

    public Vector2Int CellPosition => _grid2D.WorldToCell(transform.position);

    void Awake()
    {
        if (TryGetComponent<IGridElementDef>(out var gridElementDef))
        {
            var gridIndexType = gridElementDef.GridIndexType;
            _gridIndex = transform.root.GetComponent<GridIndexes>().GetIndex(gridIndexType);
        }

        _grid2D = transform.root.GetComponent<Grid2D>();
    }

    void Start()
    {
        if (_gridIndex != null)
            AddToGridIndex();
    }

    void OnDestroy()
    {
        if (_gridIndex != null)
            RemoveFromGridIndex();
    }

    void AddToGridIndex()
    {
        _gridIndex.Add(this);
    }

    void RemoveFromGridIndex()
    {
        _gridIndex.Remove(this);
    }
}
