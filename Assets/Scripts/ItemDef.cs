using UnityEngine;

[CreateAssetMenu(menuName = "Item/Generic")]
public class ItemDef : ScriptableObject, IGizmoDef
{
    public Sprite LowMassSprite;
    public Sprite NormalMassSprite;
    public Sprite HighMassSprite;

    public Object GizmoAsset => LowMassSprite;

    public ulong NormalMassThreshold = 100.KiloGrams();
    public ulong HighMassThreshold = 1.Ton();
}