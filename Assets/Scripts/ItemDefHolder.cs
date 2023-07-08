using UnityEngine;

public class ItemDefHolder : MonoBehaviour, IWorldLayerDef
{
    public void Initialize(ItemDef itemDef)
    {
        ItemDef = itemDef;
    }

    public ItemDef ItemDef;

    public WorldLayer WorldLayer => ItemDef.WorldLayer;

    void Awake()
    {
        name = ItemDef.name;
    }
}
