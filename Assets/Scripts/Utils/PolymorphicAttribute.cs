using UnityEngine;

public class PolymorphicAttribute : PropertyAttribute
{
    public readonly bool AllowNull;

    public PolymorphicAttribute(bool allowNull = false)
    {
        AllowNull = allowNull;
    }
}
