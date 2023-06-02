using UnityEngine;

public class StructureDefHolder : MonoBehaviour, IHealthDef, IGizmoDef
{
    public StructureDef StructureDef;

    public int MaxHealthPoints => StructureDef.MaxHealthPoints;

    public Object GizmoAsset => StructureDef != null ? StructureDef.GizmoAsset : null;
}
