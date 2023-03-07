using UnityEngine;

[CreateAssetMenu]
public class GridPositioner : ScriptableObject
{
    public Grid Grid;

    public Vector2Int WorldToCell(Vector2 position) => (Vector2Int)Grid.WorldToCell(position);
    public Vector2 CellToWorld(Vector2Int position) => Grid.CellToWorld((Vector3Int)position);
    public Vector2 CellCenterWorld(Vector2Int position) => Grid.GetCellCenterWorld((Vector3Int)position);
}
