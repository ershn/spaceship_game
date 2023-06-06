using System;
using UnityEngine;

[Serializable]
public class StructureSpriteGraphicsDef : StructureGraphicsDef
{
    public override Type RendererType => typeof(StructureSpriteGraphics);

    public Sprite Sprite;

    public Color BlueprintColor;

    public override UnityEngine.Object GizmoAsset => Sprite;
}
