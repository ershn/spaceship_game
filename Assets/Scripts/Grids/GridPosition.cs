using UnityEngine;

public class GridPosition : MonoBehaviour
{
    public GridIndexer GridIndexer;

    Grid2D _grid2D;

    public Vector2Int CellPosition => _grid2D.WorldToCell(transform.position);

    void Awake()
    {
        _grid2D = transform.root.GetComponent<Grid2D>();
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
        Debug.Log($"AddToGridIndexer: {CellPosition}, {gameObject}");
        GridIndexer.Add(this);
    }

    void RemoveFromGridIndexer()
    {
        Debug.Log($"RemoveFromGridIndexer: {CellPosition}, {gameObject}");
        GridIndexer.Remove(this);
    }
}
