using UnityEngine;

[RequireComponent(typeof(Grid))]
public class Grid2D : MonoBehaviour
{
    Grid _grid;

    void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    public Vector2Int WorldToCell(Vector2 position) =>
        (Vector2Int)_grid.WorldToCell(position);

    public Vector2 CellToWorld(Vector2Int position) =>
        _grid.CellToWorld((Vector3Int)position);

    public Vector2 CellCenterWorld(Vector2Int position) =>
        _grid.GetCellCenterWorld((Vector3Int)position);
}