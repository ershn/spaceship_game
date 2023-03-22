using System;
using UnityEngine;

[CreateAssetMenu]
public class GridMonoIndexer : GridIndexer
{
    ArrayGrid<GameObject> _grid = new(500);

    public GameObject Get(Vector2Int position) => _grid[position];

    public bool Has(Vector2Int position) => _grid[position] != null;

    public override void Add(GridPosition obj)
    {
        if (_grid[obj.CellPosition] != null)
            throw new InvalidOperationException("Grid cell already used");

        _grid[obj.CellPosition] = obj.gameObject;

        OnAdd(obj);
    }

    protected virtual void OnAdd(GridPosition obj)
    {
    }

    public override void Remove(GridPosition obj)
    {
        if (_grid[obj.CellPosition] == null)
            throw new InvalidOperationException("Grid cell empty");

        _grid[obj.CellPosition] = null;

        OnRemove(obj);
    }

    protected virtual void OnRemove(GridPosition obj)
    {
    }
}