using UnityEngine;

public class ItemDefHolder : MonoBehaviour, IGizmoDef
{
    public ItemDef ItemDef;

    public Object GizmoAsset => ItemDef != null ? ItemDef.GizmoAsset : null;
}
