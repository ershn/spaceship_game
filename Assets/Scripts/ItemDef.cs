using UnityEngine;

[CreateAssetMenu(menuName = "Item/Generic")]
public class ItemDef : EntityDef, IWorldLayerGet, IAmountModeGet
{
    public WorldLayer WorldLayer => WorldLayer.Item;

    [Header("Amount")]
    [SerializeField, SerializeReference, Polymorphic]
    AmountMode _amountMode;
    public virtual AmountMode AmountMode => _amountMode;

    [Header("Graphics")]
    public AmountSprite[] AmountSprites;
}
