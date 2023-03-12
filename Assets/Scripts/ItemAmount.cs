using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ItemDefHolder))]
public class ItemAmount : MonoBehaviour, IAmount
{
    enum AmountCategory
    {
        Zero,
        Low,
        Normal,
        High
    }

    public UnityEvent OnChangeToZeroAmount;
    public UnityEvent OnChangeToLowAmount;
    public UnityEvent OnChangeToNormalAmount;
    public UnityEvent OnChangeToHighAmount;

    ItemDef _itemDef;

    public ulong Amount = 10.KiloGrams();
    ulong _reservedAmount = 0;

    void Awake()
    {
        _itemDef = GetComponent<ItemDefHolder>().ItemDef;
    }

    void Start()
    {
        TriggerAmountChangeEvent(Amount);
    }

    public ulong Get() => Amount - _reservedAmount;

    public void Add(ulong amount)
    {
        var oldAmount = Amount;
        Amount += amount;
        TriggerAmountChangeEvent(oldAmount, Amount);
    }

    public void Reserve(ulong amount)
    {
        if (amount > Amount - _reservedAmount)
        {
            throw new ArgumentOutOfRangeException(
                "The reserved amount exceeds the current unreserved amount."
                );
        }

        _reservedAmount += amount;
    }

    public void Unreserve(ulong amount)
    {
        if (amount > _reservedAmount)
        {
            throw new ArgumentOutOfRangeException(
                "The released amount exceeds the current reserved amount."
                );
        }

        _reservedAmount -= amount;
    }

    public void Remove(ulong amount)
    {
        if (amount > _reservedAmount)
        {
            throw new ArgumentOutOfRangeException(
                "The removed amount exceeds the current reserved amount."
                );
        }

        var oldAmount = Amount;
        _reservedAmount -= amount;
        Amount -= amount;
        TriggerAmountChangeEvent(oldAmount, Amount);
    }

    AmountCategory GetAmountCategory(ulong amount)
    {
        if (amount >= _itemDef.HighAmountThreshold)
            return AmountCategory.High;
        else if (amount >= _itemDef.NormalAmountThreshold)
            return AmountCategory.Normal;
        else if (amount > 0)
            return AmountCategory.Low;
        else
            return AmountCategory.Zero;
    }

    void TriggerAmountChangeEvent(ulong amount)
    {
        TriggerAmountChangeEvent(GetAmountCategory(amount));
    }

    void TriggerAmountChangeEvent(ulong oldAmount, ulong newAmount)
    {
        var oldAmountCategory = GetAmountCategory(oldAmount);
        var newAmountCategory = GetAmountCategory(newAmount);

        if (oldAmountCategory != newAmountCategory)
            TriggerAmountChangeEvent(newAmountCategory);
    }

    void TriggerAmountChangeEvent(AmountCategory amountCategory)
    {
        if (amountCategory == AmountCategory.Zero)
            OnChangeToZeroAmount.Invoke();
        else if (amountCategory == AmountCategory.Low)
            OnChangeToLowAmount.Invoke();
        else if (amountCategory == AmountCategory.Normal)
            OnChangeToNormalAmount.Invoke();
        else
            OnChangeToHighAmount.Invoke();
    }
}
