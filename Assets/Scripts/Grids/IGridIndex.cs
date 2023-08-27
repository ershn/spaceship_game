using UnityEngine;

public interface IGridIndex
{
    void Add(Vector2Int position, GameObject gameObject);
    void Remove(Vector2Int position, GameObject gameObject);
}
