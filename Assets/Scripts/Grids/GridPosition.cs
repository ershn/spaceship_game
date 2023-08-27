using UnityEngine;

[ExecuteAlways]
public class GridPosition : MonoBehaviour
{
    IGridIndex _gridIndex;

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

    public Vector2Int CellPosition => (Vector2Int)Grid.WorldToCell(transform.position);

    void OnEnable()
    {
        if (TryGetComponent<IWorldLayerGet>(out var worldLayerConf))
        {
            var worldLayer = worldLayerConf.WorldLayer;
            _gridIndex = GetComponentInParent<GridIndexes>().GetLayerIndex(worldLayer);
            _gridIndex.Add(CellPosition, gameObject);
        }
    }

    void OnDisable()
    {
        if (_gridIndex != null)
        {
            _gridIndex.Remove(CellPosition, gameObject);
            _gridIndex = null;
        }
    }
}
