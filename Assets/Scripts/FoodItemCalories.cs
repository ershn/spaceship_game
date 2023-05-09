using UnityEngine;
using UnityEngine.Assertions;

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
        Assert.IsTrue(mass <= _itemMass.Get());

        return _foodItemDef.MassToCalories(mass);
    }

    public ulong GetMass(ulong calories)
    {
        Assert.IsTrue(calories <= TotalCalories);

        return _foodItemDef.CaloriesToMass(calories);
    }
}