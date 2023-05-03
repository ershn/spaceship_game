using System;
using UnityEngine;

[RequireComponent(typeof(Backpack))]
[RequireComponent(typeof(Stomach))]
public class FoodConsumption : MonoBehaviour, IWork
{
    public float TimePerKiloGramConsumed = 2f;

    Backpack _backpack;
    Stomach _stomach;

    float _accumulatedTime;

    void Awake()
    {
        _backpack = GetComponent<Backpack>();
        _stomach = GetComponent<Stomach>();
    }

    public bool Work(float time)
    {
        _accumulatedTime += time;
        if (_accumulatedTime < .5f)
            return false;

        var massConsumed = MassConsumed(_accumulatedTime);
        var isBackpackEmpty = !ConsumeMass(massConsumed);
        _accumulatedTime = 0f;
        return isBackpackEmpty;
    }

    ulong MassConsumed(float time) => (ulong)(time / TimePerKiloGramConsumed * 1.KiloGram());

    bool ConsumeMass(ulong targetMass)
    {
        if (!_backpack.TryFirst<FoodItemDef>(out var foodItem))
            throw new InvalidOperationException("No food in backpack");

        do
        {
            var (itemDef, itemMass) = foodItem;
            if (targetMass > itemMass)
            {
                ConsumeItem(itemDef, itemMass);
                targetMass -= itemMass;
            }
            else
            {
                ConsumeItem(itemDef, targetMass);
                return true;
            }
        } while (_backpack.TryFirst(out foodItem));

        return false;
    }

    void ConsumeItem(FoodItemDef foodItemDef, ulong mass)
    {
        var consumedCalories = foodItemDef.MassToCalories(mass);
        _stomach.StoreCalories(consumedCalories);
        _backpack.Remove(foodItemDef, mass);
    }
}