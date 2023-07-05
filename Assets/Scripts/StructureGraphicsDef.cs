using UnityEngine;

public abstract class StructureGraphicsDef : IGizmoAssetProvider
{
    public abstract void Setup(GameObject gameObject);

    public abstract Object GizmoAsset { get; }
}
