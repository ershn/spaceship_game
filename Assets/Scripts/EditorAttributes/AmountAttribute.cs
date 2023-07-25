using UnityEngine;

public class AmountAttribute : PropertyAttribute
{
    public readonly AmountType? AmountType;

    public AmountAttribute() { }

    public AmountAttribute(AmountType amountType)
    {
        AmountType = amountType;
    }
}
