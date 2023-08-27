using UnityEngine;
using UnityEngine.Tilemaps;

public class StructureTilePlacer
{
    readonly StructureTileGraphicsDef _def;
    readonly Tilemap _tilemap;
    readonly Vector3Int _position;

    public StructureTilePlacer(StructureTileGraphicsDef def, Tilemap tilemap, Transform transform)
    {
        _def = def;
        _tilemap = tilemap;
        _position = tilemap.layoutGrid.WorldToCell(transform.position);
    }

    public void Place()
    {
        _tilemap.SetTile(_position, _def.Tile);
    }

    public void ToBlueprintGraphics()
    {
        _tilemap.SetColor(_position, _def.BlueprintColor);
    }

    public void ToNormalGraphics()
    {
        _tilemap.SetColor(_position, Color.white);
    }

    public void Remove()
    {
        _tilemap.SetTile(_position, null);
    }
}
