using Unity.VisualScripting;
using UnityEngine;

public abstract class StructureDef : EntityDef, IWorldLayerGet
{
    public abstract WorldLayer WorldLayer { get; }

    [Header("Graphics")]
    [SerializeReference, Polymorphic]
    public StructureGraphicsDef StructureGraphicsDef;

    [Header("Construction")]
    public ItemDefAmount[] ComponentAmounts;
    public float ConstructionTime = 10f;
    public float DeconstructionTimeMultiplier = .5f;

    [Header("Status")]
    public bool SetupRequired = false;

    [Header("Health")]
    public int MaxHealthPoints = 100;

    [Header("Resource processing")]
    public StateGraphAsset ResourceProcessor;

    public abstract bool IsConstructibleAt(
        GridIndexes gridIndexes,
        Vector2Int cellPosition,
        bool ignoreExisting = false
    );
}
