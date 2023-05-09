using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(ItemDefHolder))]
public class ItemMass : MonoBehaviour
{
    enum MassCategory
    {
        Zero,
        Low,
        Normal,
        High
    }

    public UnityEvent OnChangeToZeroMass;
    public UnityEvent OnChangeToLowMass;
    public UnityEvent OnChangeToNormalMass;
    public UnityEvent OnChangeToHighMass;

    ItemDef _itemDef;

    public ulong Mass = 10.KiloGrams();
    ulong _reservedMass = 0;

    void Awake()
    {
        _itemDef = GetComponent<ItemDefHolder>().ItemDef;
    }

    void Start()
    {
        TriggerMassChangeEvent(Mass);
    }

    public ItemDef Def => _itemDef;

    public ulong Get() => Mass - _reservedMass;

    public void Add(ulong mass)
    {
        var oldMass = Mass;
        Mass += mass;
        TriggerMassChangeEvent(oldMass, Mass);
    }

    public void Reserve(ulong mass)
    {
        Assert.IsTrue(mass <= Mass - _reservedMass);

        _reservedMass += mass;
    }

    public void Unreserve(ulong mass)
    {
        Assert.IsTrue(mass <= _reservedMass);

        _reservedMass -= mass;
    }

    public void Remove(ulong mass)
    {
        Assert.IsTrue(mass <= _reservedMass);

        var oldMass = Mass;
        _reservedMass -= mass;
        Mass -= mass;
        TriggerMassChangeEvent(oldMass, Mass);
    }

    MassCategory GetMassCategory(ulong mass)
    {
        if (mass >= _itemDef.HighMassThreshold)
            return MassCategory.High;
        else if (mass >= _itemDef.NormalMassThreshold)
            return MassCategory.Normal;
        else if (mass > 0)
            return MassCategory.Low;
        else
            return MassCategory.Zero;
    }

    void TriggerMassChangeEvent(ulong mass)
    {
        TriggerMassChangeEvent(GetMassCategory(mass));
    }

    void TriggerMassChangeEvent(ulong oldMass, ulong newMass)
    {
        var oldMassCategory = GetMassCategory(oldMass);
        var newMassCategory = GetMassCategory(newMass);

        if (oldMassCategory != newMassCategory)
            TriggerMassChangeEvent(newMassCategory);
    }

    void TriggerMassChangeEvent(MassCategory massCategory)
    {
        if (massCategory == MassCategory.Zero)
            OnChangeToZeroMass.Invoke();
        else if (massCategory == MassCategory.Low)
            OnChangeToLowMass.Invoke();
        else if (massCategory == MassCategory.Normal)
            OnChangeToNormalMass.Invoke();
        else
            OnChangeToHighMass.Invoke();
    }
}