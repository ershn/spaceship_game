using UnityEngine;

[RequireComponent(typeof(StructureDefHolder))]
public class ConstructionWork : TransactionalWork
{
    StructureDef _structureDef;

    protected override float RequiredTime => _structureDef.ConstructionTime;

    void Awake()
    {
        _structureDef = GetComponent<StructureDefHolder>().StructureDef;
    }
}
