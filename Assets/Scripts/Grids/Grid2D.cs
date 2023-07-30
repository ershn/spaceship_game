using UnityEngine;

public class Grid2D : MonoBehaviour
{
    Grid _grid;

    void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    public Vector2Int World2DToCell(Vector2 position) => (Vector2Int)_grid.WorldToCell(position);

    public Vector2 CellToWorld2D(Vector2Int position) =>
        _grid.GetCellCenterWorld((Vector3Int)position);
}
