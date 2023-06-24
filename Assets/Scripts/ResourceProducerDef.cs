using System;
using UnityEngine;

[Serializable]
public class ResourceProducerDef
{
    [SerializeField]
    float _interval;
    public float Interval => _interval;

    [SerializeField]
    float _probability;
    public float Probability => _probability;

    [SerializeField]
    ItemDef _itemDef;
    public ItemDef ItemDef => _itemDef;

    [SerializeField]
    ulong _amount;
    public ulong Amount => _amount;
}
