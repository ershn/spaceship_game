using System;
using System.Collections.Generic;
using UnityEngine;

public static class ItemEnumerableExtensions
{
    public static IEnumerable<(ItemMass itemMass, ulong markedMass)> CumulateMass(
        this IEnumerable<GameObject> items, ulong totalMass
        ) =>
        SelectMass(items, itemMass =>
        {
            var markedMass = Math.Min(totalMass, itemMass.Get());
            totalMass -= markedMass;
            return (totalMass == 0, markedMass);
        });

    public static IEnumerable<(ItemMass itemMass, ulong markedMass)> CumulateCalories(
        this IEnumerable<GameObject> items, ulong totalCalories
        ) =>
        SelectMass(items, itemMass =>
        {
            var itemCalories = itemMass.GetComponent<FoodItemCalories>();
            ulong markedMass;
            if (itemCalories.TotalCalories <= totalCalories)
            {
                markedMass = itemMass.Get();
                totalCalories -= itemCalories.TotalCalories;
            }
            else
            {
                markedMass = itemCalories.GetMass(totalCalories);
                totalCalories = 0;
            }
            return (totalCalories == 0, markedMass);
        });

    static IEnumerable<(ItemMass itemMass, ulong markedMass)> SelectMass(
        IEnumerable<GameObject> items, Func<ItemMass, (bool, ulong)> selector
        )
    {
        var selectedItems = new List<(ItemMass, ulong)>();

        foreach (var item in items)
        {
            var itemMass = item.GetComponent<ItemMass>();
            if (itemMass.Get() == 0)
                continue;
            var (stop, markedMass) = selector(itemMass);
            selectedItems.Add((itemMass, markedMass));
            if (stop)
                break;
        }

        return selectedItems;
    }
}