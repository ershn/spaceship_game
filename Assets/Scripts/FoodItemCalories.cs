using UnityEngine;
using UnityEngine.Assertions;

public class FoodItemCalories : MonoBehaviour
{
    FoodItemDef _foodItemDef;
    ItemAmount _itemAmount;

    void Awake()
    {
        _foodItemDef = GetComponent<FoodItemDefHolder>().FoodItemDef;
        _itemAmount = GetComponent<ItemAmount>();
    }

    public ulong TotalCalories => _foodItemDef.MassToCalories(_itemAmount.Amount);

    public ulong GetCalories(ulong mass)
    {
        Assert.IsTrue(mass <= _itemAmount.Amount);

        return _foodItemDef.MassToCalories(mass);
    }

    public ulong GetMass(ulong calories)
    {
        Assert.IsTrue(calories <= TotalCalories);

        return _foodItemDef.CaloriesToMass(calories);
    }
}
