using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class StructureTileGraphicsDef : StructureGraphicsDef
{
    public TileBase BlueprintTile;
    public TileBase NormalTile;

    public override void Setup(GameObject gameObject) =>
        gameObject.AddComponent<StructureTileGraphics>();

    public override UnityEngine.Object GizmoAsset => NormalTile;
}
