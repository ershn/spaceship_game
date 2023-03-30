using UnityEngine;

[RequireComponent(typeof(BuildingDefHolder))]
public class ConstructionWork : AbstractWork
{
    BuildingDef _buildingDef;

    protected override float RequiredTime => _buildingDef.ConstructionTime;

    void Awake()
    {
        _buildingDef = GetComponent<BuildingDefHolder>().BuildingDef;
    }
}