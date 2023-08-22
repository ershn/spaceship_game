using Unity.VisualScripting;
using UnityEngine;

public abstract class StructureDef : ScriptableObject, ITemplatedPrefab, IWorldLayerGet
{
    public abstract WorldLayer WorldLayer { get; }

    [Header("Instantiation")]
    [SerializeField]
    GameObject _template;
    public GameObject Template => _template;

    [SerializeField]
    GameObject _prefab;
    public GameObject Prefab
    {
        get => _prefab;
        set => _prefab = value;
    }

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
