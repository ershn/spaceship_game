using UnityEngine;

[CreateAssetMenu(menuName = "Item/Generic")]
public class ItemDef : ScriptableObject, IWorldLayerGet, IAmountModeGet
{
    public WorldLayer WorldLayer => WorldLayer.Item;

    public GameObject Prefab;

    [SerializeField, SerializeReference, Polymorphic]
    AmountMode _amountMode;
    public virtual AmountMode AmountMode => _amountMode;

    public AmountSprite[] AmountSprites;
}
