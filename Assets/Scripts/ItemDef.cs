using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Generic")]
public class ItemDef : ScriptableObject, IGizmoDef
{
    [SerializeReference, Polymorphic]
    public AmountAddressingMode AmountAddressingMode;

    public AmountSprite[] AmountSprites;

    public Object GizmoAsset => AmountSprites.FirstOrDefault().Sprite;
}
