using Unity.VisualScripting;
using UnityEngine;

public abstract class StructureDef : ScriptableObject, IWorldLayerMemberConf
{
    public abstract WorldLayer WorldLayer { get; }

    [Header("Instantiation")]
    public GameObject Prefab;

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

    public abstract bool IsConstructibleAt(Vector2Int cellPosition, GridIndexes gridIndexes);
}
