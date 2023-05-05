using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemEnumerableExtensions
{
    public static IEnumerable<(ItemMass itemMass, ulong markedMass)>
        CumulateMass(this IEnumerable<GameObject> items, ulong totalMass) =>
        items
        .Select(gameObject => gameObject.GetComponent<ItemMass>())
        .Where(itemMass => itemMass.Get() > 0)
        .SelectWhile(itemMass =>
        {
            var markedMass = Math.Min(totalMass, itemMass.Get());
            totalMass -= markedMass;
            return (totalMass > 0, (itemMass, markedMass));
        });

    public static IEnumerable<(FoodItemCalories itemCalories, ulong markedCalories)>
        CumulateCalories(this IEnumerable<GameObject> items, ulong totalCalories) =>
        items
        .Select(gameObject => gameObject.GetComponent<FoodItemCalories>())
        .Where(itemCalories => itemCalories.TotalCalories > 0)
        .SelectWhile(itemCalories =>
        {
            var markedCalories = Math.Min(totalCalories, itemCalories.TotalCalories);
            totalCalories -= markedCalories;
            return (totalCalories > 0, (itemCalories, markedCalories));
        });

    public static IEnumerable<(ItemMass itemMass, ulong markedMass)> CaloriesToMass(
        this IEnumerable<(FoodItemCalories itemCalories, ulong markedCalories)> items
        ) =>
        items.Select(item =>
        (
            item.itemCalories.GetComponent<ItemMass>(),
            item.itemCalories.GetMass(item.markedCalories)
        ));
}