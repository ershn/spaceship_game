using UnityEngine;

[RequireComponent(typeof(BuildingDefHolder))]
public class DeconstructionWork : AbstractWork
{
    BuildingDef _buildingDef;

    protected override float RequiredTime =>
        _buildingDef.ConstructionTime * _buildingDef.DeconstructionTimeMultiplier;

    void Awake()
    {
        _buildingDef = GetComponent<BuildingDefHolder>().BuildingDef;
    }
}