using UnityEngine;

[RequireComponent(typeof(StructureDefHolder))]
public class StructureGizmoAssetProvider : MonoBehaviour, IGizmoAssetProvider
{
    public Object GizmoAsset
    {
        get
        {
            var structureDef = GetComponent<StructureDefHolder>().StructureDef;
            if (structureDef == null)
                return null;

            return structureDef.StructureGraphicsDef.GizmoAsset;
        }
    }
}
