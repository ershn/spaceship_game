using System;
using UnityEngine;

[Serializable]
public struct AmountSprite
{
    [Amount]
    public ulong MinAmount;

    public Sprite Sprite;
}
