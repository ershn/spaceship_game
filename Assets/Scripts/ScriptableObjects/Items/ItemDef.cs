using UnityEngine;

[CreateAssetMenu]
public class ItemDef : ScriptableObject, IGizmoDef
{
    public Sprite LowAmountSprite;
    public Sprite NormalAmountSprite;
    public Sprite HighAmountSprite;

    public Object GizmoAsset => LowAmountSprite;

    public ulong NormalAmountThreshold = 100.KiloGrams();
    public ulong HighAmountThreshold = 1.Ton();
}