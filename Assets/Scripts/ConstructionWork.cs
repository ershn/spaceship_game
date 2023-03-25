using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BuildingDefHolder))]
public class ConstructionWork : MonoBehaviour, IWork
{
    enum Phase
    {
        Pending,
        Started,
        Completed
    }

    public UnityEvent OnConstructionStarted;
    public FloatEvent OnConstructionProgress;
    public UnityEvent OnConstructionCompleted;

    BuildingDef _buildingDef;

    float RequiredTime => _buildingDef.ConstructionTime;
    float _currentTime = 0f;

    Phase _phase = Phase.Pending;

    void Awake()
    {
        _buildingDef = GetComponent<BuildingDefHolder>().BuildingDef;
    }

    public bool AddWorkTime(float time)
    {
        if (_phase == Phase.Completed)
            throw new InvalidOperationException("Construction work already completed");

        if (_phase == Phase.Pending)
        {
            _phase = Phase.Started;
            OnConstructionStarted.Invoke();
        }

        _currentTime += time;

        var progress = Mathf.Clamp01(_currentTime / RequiredTime);
        OnConstructionProgress.Invoke(progress);

        if (progress >= 1f)
        {
            _phase = Phase.Completed;
            OnConstructionCompleted.Invoke();
        }

        return _phase == Phase.Completed;
    }
}