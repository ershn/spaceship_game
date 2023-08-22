using UnityEngine;

public class StructureTileGraphics : MonoBehaviour, ITemplate<StructureTileGraphicsDef>
{
    public void Template(StructureTileGraphicsDef def)
    {
        _tileGraphicsDef = def;
    }

    [SerializeField, HideInInspector]
    StructureTileGraphicsDef _tileGraphicsDef;

    StructureTilePlacer _tilePlacer;

    void Awake()
    {
        _tilePlacer = StructureTilePlacer.TryCreate(_tileGraphicsDef, gameObject);

        var structureGraphics = transform.parent.GetComponent<StructureGraphics>();
        structureGraphics.OnConstructionCompleted += OnConstructionCompleted;
    }

    void Start()
    {
        _tilePlacer.Place();
        _tilePlacer.ToBlueprintGraphics();
    }

    void OnDestroy()
    {
        _tilePlacer.Remove();
    }

    void OnConstructionCompleted() => _tilePlacer.ToNormalGraphics();
}
