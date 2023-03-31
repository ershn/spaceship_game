using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid/Poly")]
public class GridPolyIndexer : GridIndexer
{
    ArrayGrid<List<GameObject>> _grid = new(500);

    public IEnumerable<GameObject> Get(Vector2Int position) => _grid[position] ?? new();

    public override void Add(GridPosition obj)
    {
        var list = _grid[obj.CellPosition];
        if (list == null)
        {
            list = new();
            _grid[obj.CellPosition] = list;
        }
        list.Add(obj.gameObject);

        OnAdd(obj);
    }

    protected virtual void OnAdd(GridPosition obj)
    {
    }

    public override void Remove(GridPosition obj)
    {
        var list = _grid[obj.CellPosition];
        if (list == null)
            throw new InvalidOperationException("Grid cell empty");

        if (!list.Remove(obj.gameObject))
            throw new InvalidOperationException("Object not in grid cell");

        if (list.Count == 0)
            _grid[obj.CellPosition] = null;

        OnRemove(obj);
    }

    protected virtual void OnRemove(GridPosition obj)
    {
    }
}