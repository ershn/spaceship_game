using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(IHealthDef))]
public class Health : MonoBehaviour
{
    public UnityEvent OnZeroHealth;
    public UnityEvent OnMaxHealth;

    IHealthDef _healthDef;

    public int MaxHealthPoints { get => _healthDef.MaxHealthPoints; }
    public int HealthPoints { get; private set; } = 1;

    void Awake()
    {
        _healthDef = GetComponent<IHealthDef>();
    }

    public void AddHealth(int points)
    {
        HealthPoints = Mathf.Clamp(HealthPoints + points, 0, MaxHealthPoints);

        NotifyHealthChange();
    }

    public void SetHealthPercentage(int percentage)
    {
        HealthPoints = MaxHealthPoints * percentage / 100;

        NotifyHealthChange();
    }

    void NotifyHealthChange()
    {
        if (HealthPoints == 0)
            OnZeroHealth.Invoke();
        else if (HealthPoints == MaxHealthPoints)
            OnMaxHealth.Invoke();
    }
}