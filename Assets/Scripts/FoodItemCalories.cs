using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(ItemDefHolder))]
[RequireComponent(typeof(ItemAmount))]
public class FoodItemCalories : MonoBehaviour
{
    FoodItemDef _foodItemDef;
    ItemAmount _itemAmount;

    void Awake()
    {
        _foodItemDef = (FoodItemDef)GetComponent<ItemDefHolder>().ItemDef;
        _itemAmount = GetComponent<ItemAmount>();
    }

    public ulong TotalCalories => _foodItemDef.MassToCalories(_itemAmount.Get());

    public ulong GetCalories(ulong mass)
    {
        Assert.IsTrue(mass <= _itemAmount.Get());

        return _foodItemDef.MassToCalories(mass);
    }

    public ulong GetMass(ulong calories)
    {
        Assert.IsTrue(calories <= TotalCalories);

        return _foodItemDef.CaloriesToMass(calories);
    }
}
