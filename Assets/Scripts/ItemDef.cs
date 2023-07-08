using UnityEngine;

[CreateAssetMenu(menuName = "Item/Generic")]
public class ItemDef : ScriptableObject, IWorldLayerDef
{
    public WorldLayer WorldLayer => WorldLayer.Item;

    public GameObject Prefab;

    [SerializeReference, Polymorphic]
    public AmountAddressingMode AmountAddressingMode;

    public AmountSprite[] AmountSprites;
}
