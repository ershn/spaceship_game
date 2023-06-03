using UnityEngine;

public class StructureDefHolder : MonoBehaviour, IHealthDef
{
    public StructureDef StructureDef;

    public int MaxHealthPoints => StructureDef.MaxHealthPoints;
}
