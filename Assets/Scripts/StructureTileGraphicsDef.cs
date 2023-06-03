using System;
using UnityEngine.Tilemaps;

[Serializable]
public class StructureTileGraphicsDef : StructureGraphicsDef
{
    public TileBase BlueprintTile;
    public TileBase NormalTile;

    public override UnityEngine.Object GizmoAsset => NormalTile;
}
