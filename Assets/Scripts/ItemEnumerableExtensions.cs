using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemEnumerableExtensions
{
    public static IEnumerable<(ItemAmount itemAmount, ulong markedAmount)> CumulateAmount(
        this IEnumerable<GameObject> items,
        ulong totalAmount
    ) =>
        items
            .Select(gameObject => gameObject.GetComponent<ItemAmount>())
            .Where(itemAmount => itemAmount.Get() > 0)
            .SelectWhile(itemAmount =>
            {
                var markedAmount = Math.Min(totalAmount, itemAmount.Get());
                totalAmount -= markedAmount;
                return (totalAmount > 0, (itemAmount, markedAmount));
            })
            .ToArray();

    public static IEnumerable<(
        FoodItemCalories itemCalories,
        ulong markedCalories
    )> CumulateCalories(this IEnumerable<GameObject> items, ulong totalCalories) =>
        items
            .Select(gameObject => gameObject.GetComponent<FoodItemCalories>())
            .Where(itemCalories => itemCalories.TotalCalories > 0)
            .SelectWhile(itemCalories =>
            {
                var markedCalories = Math.Min(totalCalories, itemCalories.TotalCalories);
                totalCalories -= markedCalories;
                return (totalCalories > 0, (itemCalories, markedCalories));
            })
            .ToArray();

    public static IEnumerable<(ItemAmount itemAmount, ulong markedMass)> CaloriesToMass(
        this IEnumerable<(FoodItemCalories itemCalories, ulong markedCalories)> items
    ) =>
        items.Select(item =>
        {
            var (itemCalories, markedCalories) = item;
            return (itemCalories.GetComponent<ItemAmount>(), itemCalories.GetMass(markedCalories));
        });
}
