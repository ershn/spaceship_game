using UnityEngine;

public class ItemDefHolder : MonoBehaviour, IGridElementDef
{
    public ItemDef ItemDef;

    public GridIndexType GridIndexType => ItemDef.GridIndexType;
}
