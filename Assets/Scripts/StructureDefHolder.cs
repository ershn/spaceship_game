using UnityEngine;

public class StructureDefHolder : MonoBehaviour, IGridElementDef, IHealthDef
{
    public StructureDef StructureDef;

    public GridIndexType GridIndexType => StructureDef.GridIndexType;

    public int MaxHealthPoints => StructureDef.MaxHealthPoints;
}
