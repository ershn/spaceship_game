using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacer : MonoBehaviour
{
    public GridPositioner GridPositioner;
    public GridIndexer GridIndexer;

    public Vector2Int CellPosition { get; private set; }

    void Start()
    {
        AddToGameGrid();
    }

    void OnDestroy()
    {
        RemoveFromGameGrid();
    }

    void AddToGameGrid()
    {
        CellPosition = GridPositioner.WorldToCell(transform.position);
        Debug.Log($"AddToGameGrid: {CellPosition}, {gameObject}");
        GridIndexer.Add(CellPosition, gameObject);
    }

    void RemoveFromGameGrid()
    {
        Debug.Log($"RemoveFromGameGrid: {CellPosition}, {gameObject}");
        GridIndexer.Remove(CellPosition);
        CellPosition = default;
    }
}
