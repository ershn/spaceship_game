using System;

public abstract class StructureGraphicsDef : IGizmoAssetProvider
{
    public abstract Type RendererType { get; }

    public abstract UnityEngine.Object GizmoAsset { get; }
}
