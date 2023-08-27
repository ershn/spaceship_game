using UnityEngine;
using UnityEngine.Assertions;

public class GOGridIndex : IGridIndex
{
    readonly ArrayGrid<GameObject> _grid = new(500);
    readonly IGridIndex _twinGrid;

    public GOGridIndex(IGridIndex twinGrid = null)
    {
        _twinGrid = twinGrid;
    }

    public GameObject Get(Vector2Int position)
    {
        var obj = _grid[position];
        Assert.IsNotNull(obj);

        return obj;
    }

    public bool TryGet(Vector2Int position, out GameObject obj)
    {
        obj = _grid[position];
        return obj != null;
    }

    public bool Has(Vector2Int position) => _grid[position] != null;

    public virtual void Add(Vector2Int position, GameObject gameObject)
    {
        Assert.IsNull(_grid[position]);

        _grid[position] = gameObject;

        _twinGrid?.Add(position, gameObject);
    }

    public virtual void Remove(Vector2Int position, GameObject gameObject)
    {
        Assert.IsNotNull(_grid[position]);

        _grid[position] = null;

        _twinGrid?.Remove(position, gameObject);
    }
}
