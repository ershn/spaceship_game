using UnityEngine;

[CreateAssetMenu(menuName = "Item/Food")]
public class FoodItemDef : ItemDef
{
    readonly MassAddressingMode _massAddressingMode = new();
    public override AmountAddressingMode AmountAddressingMode => _massAddressingMode;

    public uint GramToCaloriesMultiplier = 1000;

    public ulong MassToCalories(ulong mass) => mass / 1.Gram() * GramToCaloriesMultiplier;

    public ulong CaloriesToMass(ulong calories) => calories / GramToCaloriesMultiplier * 1.Gram();
}
