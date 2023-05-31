using UnityEngine;

public abstract class GridIndexer : ScriptableObject
{
    public abstract void Add(GridPosition obj);
    public abstract void Remove(GridPosition obj);
}
