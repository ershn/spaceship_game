using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GOListGridIndex : IGridIndex
{
    readonly ArrayGrid<List<GameObject>> _grid = new(500);
    readonly IGridIndex _twinGrid;

    public GOListGridIndex(IGridIndex twinGrid = null)
    {
        _twinGrid = twinGrid;
    }

    public IEnumerable<GameObject> Get(Vector2Int position) => _grid[position] ?? new();

    public virtual void Add(Vector2Int position, GameObject gameObject)
    {
        var list = _grid[position];
        if (list == null)
        {
            list = new();
            _grid[position] = list;
        }
        list.Add(gameObject);

        _twinGrid?.Add(position, gameObject);
    }

    public virtual void Remove(Vector2Int position, GameObject gameObject)
    {
        var list = _grid[position];
        Assert.IsNotNull(list);

        var removed = list.Remove(gameObject);
        Assert.IsTrue(removed);

        if (list.Count == 0)
            _grid[position] = null;

        _twinGrid?.Remove(position, gameObject);
    }
}
