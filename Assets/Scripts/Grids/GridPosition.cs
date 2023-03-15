using UnityEngine;

public class GridPosition : MonoBehaviour
{
    public GridPositioner GridPositioner;
    public GridIndexer GridIndexer;

    public Vector2Int CellPosition { get; private set; }

    void Start()
    {
        CellPosition = GridPositioner.WorldToCell(transform.position);
        AddToGridIndexer();
    }

    void OnDestroy()
    {
        RemoveFromGridIndexer();
        CellPosition = default;
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
