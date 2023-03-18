using UnityEngine;

public class BuildingDefHolder : MonoBehaviour, IHealthDef, IGizmoDef
{
    public BuildingDef BuildingDef;

    public int MaxHealthPoints => BuildingDef.MaxHealthPoints;

    public Object GizmoAsset => BuildingDef != null ? BuildingDef.GizmoAsset : null;
}