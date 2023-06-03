using UnityEngine;

[CreateAssetMenu(menuName = "Item/Generic")]
public class ItemDef : ScriptableObject
{
    [SerializeReference, Polymorphic]
    public AmountAddressingMode AmountAddressingMode;

    public AmountSprite[] AmountSprites;
}
