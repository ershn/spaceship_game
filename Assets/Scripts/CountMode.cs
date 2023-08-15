using System;
using UnityEngine;

[Serializable]
public class CountMode : AmountMode
{
    [SerializeField, Amount(AmountType.Mass)]
    ulong _unitMass;

    public override AmountType AmountType => AmountType.Count;

    public override ulong AmountToMass(ulong count) => _unitMass * count;
}
