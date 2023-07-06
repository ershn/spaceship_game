using UnityEngine;

public class ItemDefHolder : MonoBehaviour, IWorldLayerDef
{
    public ItemDef ItemDef;

    public WorldLayer WorldLayer => ItemDef.WorldLayer;

    void Awake()
    {
        name = ItemDef.name;
    }
}
