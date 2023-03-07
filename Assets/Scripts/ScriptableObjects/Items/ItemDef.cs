using UnityEngine;

[CreateAssetMenu]
public class ItemDef : ScriptableObject
{
    public Sprite LowAmountSprite;
    public Sprite NormalAmountSprite;
    public Sprite HighAmountSprite;

    public ulong NormalAmountThreshold = 100.KiloGrams();
    public ulong HighAmountThreshold = 1.Ton();
}