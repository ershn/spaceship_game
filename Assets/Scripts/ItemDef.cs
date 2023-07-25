using UnityEngine;

[CreateAssetMenu(menuName = "Item/Generic")]
public class ItemDef : ScriptableObject, IWorldLayerMemberConf, IAmountHolderConf
{
    public WorldLayer WorldLayer => WorldLayer.Item;

    public GameObject Prefab;

    [SerializeField, SerializeReference, Polymorphic]
    AmountAddressingMode _amountAddressingMode;
    public virtual AmountAddressingMode AmountAddressingMode => _amountAddressingMode;

    public AmountSprite[] AmountSprites;
}
