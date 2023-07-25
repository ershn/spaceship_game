using UnityEngine;

public class ItemDefHolder : MonoBehaviour, IWorldLayerMemberConf, IAmountHolderConf
{
    public void Initialize(ItemDef itemDef)
    {
        ItemDef = itemDef;
    }

    public ItemDef ItemDef;

    public WorldLayer WorldLayer => ItemDef.WorldLayer;

    public AmountAddressingMode AmountAddressingMode => ItemDef.AmountAddressingMode;

    void Awake()
    {
        name = ItemDef.name;
    }
}
