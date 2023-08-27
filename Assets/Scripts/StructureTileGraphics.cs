using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class StructureTileGraphics : MonoBehaviour, ITemplate<StructureTileGraphicsDef>
{
    public void Template(StructureTileGraphicsDef def)
    {
        _tileGraphicsDef = def;
    }

    [SerializeField, HideInInspector]
    StructureTileGraphicsDef _tileGraphicsDef;

    StructureTilePlacer _tilePlacer;
    StructureGraphics _structureGraphics;

    void OnEnable()
    {
        _tilePlacer = new StructureTilePlacer(_tileGraphicsDef, GetTilemap(), transform);

        _structureGraphics = transform.parent.GetComponent<StructureGraphics>();
        _structureGraphics.OnConstructionCompleted += OnConstructionCompleted;
    }

    void OnDisable()
    {
        _structureGraphics.OnConstructionCompleted -= OnConstructionCompleted;
    }

    Tilemap GetTilemap()
    {
        var worldIO = GetComponentInParent<WorldInternalIO>();
        if (worldIO != null)
            return worldIO.Tilemap;
        return GetComponentInParent<Tilemap>();
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
