using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingDefHolder))]
public class ConstructionWork : MonoBehaviour, IWork
{
    public IntEvent OnConstructionProgressPercentage;

    BuildingDef _buildingDef;

    float RequiredTime { get => _buildingDef.ConstructionTime; }
    float _currentTime = 0f;

    void Awake()
    {
        _buildingDef = GetComponent<BuildingDefHolder>().BuildingDef;
    }

    public bool AddWorkTime(float time)
    {
        _currentTime += time;

        var progress = Mathf.Clamp(Mathf.RoundToInt(_currentTime / RequiredTime * 100), 0, 100);
        OnConstructionProgressPercentage.Invoke(progress);

        return progress == 100;
    }
}