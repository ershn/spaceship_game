using UnityEngine;

public class GridPosition : MonoBehaviour
{
    GridIndex _gridIndex;

    Grid _grid;
    Grid Grid
    {
        get
        {
            if (_grid == null)
                _grid = GetComponentInParent<Grid>();
            return _grid;
        }
    }

    void Awake()
    {
        if (TryGetComponent<IWorldLayerGet>(out var worldLayerConf))
        {
            var worldLayer = worldLayerConf.WorldLayer;
            _gridIndex = GetComponentInParent<GridIndexes>().GetLayerIndex(worldLayer);
        }
    }

    public Vector2Int CellPosition => (Vector2Int)Grid.WorldToCell(transform.position);

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
