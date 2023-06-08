using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tile/Connecting")]
public class ConnectingTile : TileBase
{
    // The sprite index in the array by the border type
    enum SpriteIndex
    {
        None,
        North,
        East,
        South,
        West,
        NorthEast,
        NorthSouth,
        EastSouth,
        EastWest,
        SouthWest,
        WestNorth,
        NorthEastSouth,
        EastSouthWest,
        SouthWestNorth,
        WestNorthEast,
        NorthEastSouthWest
    };

    public Sprite[] Sprites;
    public Color Color;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = GetSprite(position, tilemap);
        tileData.color = Color;
        tileData.flags = TileFlags.LockColor;
    }

    Sprite GetSprite(Vector3Int position, ITilemap tilemap)
    {
        int mask = 0;
        mask += tilemap.GetTile(position + new Vector3Int(0, 1, 0)) == this ? 1 : 0;
        mask += tilemap.GetTile(position + new Vector3Int(1, 0, 0)) == this ? 2 : 0;
        mask += tilemap.GetTile(position + new Vector3Int(0, -1, 0)) == this ? 4 : 0;
        mask += tilemap.GetTile(position + new Vector3Int(-1, 0, 0)) == this ? 8 : 0;

        return mask switch
        {
            0 => Sprites[(uint)SpriteIndex.NorthEastSouthWest],
            1 => Sprites[(uint)SpriteIndex.EastSouthWest],
            2 => Sprites[(uint)SpriteIndex.SouthWestNorth],
            3 => Sprites[(uint)SpriteIndex.SouthWest],
            4 => Sprites[(uint)SpriteIndex.WestNorthEast],
            5 => Sprites[(uint)SpriteIndex.EastWest],
            6 => Sprites[(uint)SpriteIndex.WestNorth],
            7 => Sprites[(uint)SpriteIndex.West],
            8 => Sprites[(uint)SpriteIndex.NorthEastSouth],
            9 => Sprites[(uint)SpriteIndex.EastSouth],
            10 => Sprites[(uint)SpriteIndex.NorthSouth],
            11 => Sprites[(uint)SpriteIndex.South],
            12 => Sprites[(uint)SpriteIndex.NorthEast],
            13 => Sprites[(uint)SpriteIndex.East],
            14 => Sprites[(uint)SpriteIndex.North],
            15 => Sprites[(uint)SpriteIndex.None],
            _ => throw new System.NotImplementedException(),
        };
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int xDelta = -1; xDelta <= 1; xDelta++)
        {
            for (int yDelta = -1; yDelta <= 1; yDelta++)
            {
                var finalPosition = position + new Vector3Int(xDelta, yDelta, 0);
                if (tilemap.GetTile(finalPosition) == this)
                    tilemap.RefreshTile(finalPosition);
            }
        }
    }
}
