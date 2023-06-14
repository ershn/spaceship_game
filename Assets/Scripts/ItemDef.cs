using UnityEngine;

[CreateAssetMenu(menuName = "Item/Generic")]
public class ItemDef : ScriptableObject, IWorldLayerDef
{
    public WorldLayer WorldLayer => WorldLayer.Item;

    [SerializeReference, Polymorphic]
    public AmountAddressingMode AmountAddressingMode;

    public AmountSprite[] AmountSprites;
}
