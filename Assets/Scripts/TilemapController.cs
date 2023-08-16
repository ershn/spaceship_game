using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapController : MonoBehaviour
{
    Tilemap _tilemap;

    void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    public void SetTile(Vector2Int cellPosition, TileBase tile, Color color)
    {
        _tilemap.SetTile((Vector3Int)cellPosition, tile);
        _tilemap.SetColor((Vector3Int)cellPosition, color);
    }

    public void UnsetTile(Vector2Int cellPosition)
    {
        _tilemap.SetTile((Vector3Int)cellPosition, null);
    }
}
