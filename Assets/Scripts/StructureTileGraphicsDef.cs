using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class StructureTileGraphicsDef : StructureGraphicsDef
{
    public TileBase Tile;
    public Color BlueprintColor;

    public override void Setup(GameObject gameObject) =>
        gameObject.AddComponent<StructureTileGraphics>();

    public override UnityEngine.Object GizmoAsset => Tile;
}
