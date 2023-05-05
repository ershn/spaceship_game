using System;
using UnityEngine;

[RequireComponent(typeof(ItemDefHolder))]
[RequireComponent(typeof(ItemMass))]
public class FoodItemCalories : MonoBehaviour
{
    FoodItemDef _foodItemDef;
    ItemMass _itemMass;

    void Awake()
    {
        _foodItemDef = (FoodItemDef)GetComponent<ItemDefHolder>().ItemDef;
        _itemMass = GetComponent<ItemMass>();
    }

    public ulong TotalCalories => _foodItemDef.MassToCalories(_itemMass.Get());

    public ulong GetCalories(ulong mass)
    {
        if (mass > _itemMass.Get())
        {
            throw new ArgumentOutOfRangeException(
                "The specified mass exceeds the current total"
                );
        }

        return _foodItemDef.MassToCalories(mass);
    }

    public ulong GetMass(ulong calories)
    {
        if (calories > TotalCalories)
        {
            throw new ArgumentOutOfRangeException(
                "The specified calorie amount exceeds the current total"
                );
        }

        return _foodItemDef.CaloriesToMass(calories);
    }
}