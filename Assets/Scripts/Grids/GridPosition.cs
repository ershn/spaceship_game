using UnityEngine;

public class GridPosition : MonoBehaviour
{
    public Grid2D Grid2D;
    public GridIndexer GridIndexer;

    Vector2Int _cellPosition;
    public Vector2Int CellPosition
    {
        get
        {
            if (Grid2D != null)
                _cellPosition = Grid2D.WorldToCell(transform.position);
            return _cellPosition;
        }
    }

    void Start()
    {
        if (GridIndexer != null)
            AddToGridIndexer();
    }

    void OnDestroy()
    {
        if (GridIndexer != null)
            RemoveFromGridIndexer();
    }

    void AddToGridIndexer()
    {
        Debug.Log($"AddToGameGrid: {CellPosition}, {gameObject}");
        GridIndexer.Add(this);
    }

    void RemoveFromGridIndexer()
    {
        Debug.Log($"RemoveFromGameGrid: {CellPosition}, {gameObject}");
        GridIndexer.Remove(this);
    }
}
