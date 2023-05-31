using UnityEngine;

public class CountAddressingMode : AmountAddressingMode
{
    [SerializeField]
    ulong _unitMass;

    public override ulong AmountToMass(ulong count) => _unitMass * count;
}