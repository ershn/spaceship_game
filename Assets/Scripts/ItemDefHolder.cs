using UnityEngine;

public class ItemDefHolder : MonoBehaviour, IWorldLayerGet, IAmountModeGet
{
    public void Initialize(ItemDef itemDef)
    {
        ItemDef = itemDef;
    }

    public ItemDef ItemDef;

    public WorldLayer WorldLayer => ItemDef.WorldLayer;

    // Used in editor
    public AmountMode AmountMode => ItemDef != null ? ItemDef.AmountMode : null;

    void Awake()
    {
        name = ItemDef.name;
    }
}
