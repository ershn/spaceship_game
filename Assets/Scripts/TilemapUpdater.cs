using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapUpdater : MonoBehaviour
{
    Tilemap _tilemap;

    void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    public void SetTile(Vector2Int cellPosition, TileBase tile)
    {
        Debug.Log($"SetTile: {cellPosition}, {tile}");
        _tilemap.SetTile((Vector3Int)cellPosition, tile);
    }

    public void UnsetTile(Vector2Int cellPosition)
    {
        Debug.Log($"UnsetTile: {cellPosition}");
        _tilemap.SetTile((Vector3Int)cellPosition, null);
    }
}