using UnityEngine;

[ExecuteAlways]
public class EditorStructureTileRenderer : MonoBehaviour, ITemplate<StructureTileGraphicsDef>
{
    public void Template(StructureTileGraphicsDef def)
    {
        _tileGraphicsDef = def;
    }

    [SerializeField, HideInInspector]
    StructureTileGraphicsDef _tileGraphicsDef;

    StructureTilePlacer _tilePlacer;

    void Update()
    {
        if (Application.IsPlaying(gameObject))
            return;

        if (_tilePlacer == null)
        {
            _tilePlacer = StructureTilePlacer.TryCreate(_tileGraphicsDef, gameObject);
            _tilePlacer?.Place();
        }
    }

    void OnDestroy()
    {
        if (Application.IsPlaying(gameObject))
            return;

        _tilePlacer?.Remove();
    }
}
