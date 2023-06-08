using UnityEngine;

[CreateAssetMenu(menuName = "Item/Generic")]
public class ItemDef : ScriptableObject, IGridElementDef
{
    public GridIndexType GridIndexType => GridIndexType.ItemGrid;

    [SerializeReference, Polymorphic]
    public AmountAddressingMode AmountAddressingMode;

    public AmountSprite[] AmountSprites;
}
