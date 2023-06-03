using UnityEngine;

public class StructureDef : ScriptableObject
{
    [SerializeReference, Polymorphic]
    public StructureGraphicsDef StructureGraphicsDef;

    public ItemDefAmount[] ComponentAmounts;

    public int MaxHealthPoints = 100;

    public float ConstructionTime = 10f;
    public float DeconstructionTimeMultiplier = .5f;
}
