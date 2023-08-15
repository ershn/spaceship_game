using UnityEngine;

public class GridPosition : MonoBehaviour
{
    GridIndex _gridIndex;

    Grid2D _grid2D;

    public Vector2Int CellPosition => _grid2D.World2DToCell(transform.position);

    void Awake()
    {
        if (TryGetComponent<IWorldLayerGet>(out var worldLayerConf))
        {
            var worldLayer = worldLayerConf.WorldLayer;
            _gridIndex = GetComponentInParent<GridIndexes>().GetLayerIndex(worldLayer);
        }

        _grid2D = GetComponentInParent<Grid2D>();
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
