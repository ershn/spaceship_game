using UnityEngine;

[RequireComponent(typeof(BuildingDefHolder))]
public class ConstructionWork : TransactionalWork
{
    BuildingDef _buildingDef;

    protected override float RequiredTime => _buildingDef.ConstructionTime;

    void Awake()
    {
        _buildingDef = GetComponent<BuildingDefHolder>().BuildingDef;
    }
}