using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class StructureSpriteGraphics : MonoBehaviour, IStructureGraphics
{
    SpriteRenderer _spriteRenderer;
    StructureSpriteGraphicsDef _spriteGraphicsDef;

    void Awake()
    {
        if (!TryGetComponent(out _spriteRenderer))
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        var structureDef = GetComponent<StructureDefHolder>().StructureDef;
        _spriteGraphicsDef = (StructureSpriteGraphicsDef)structureDef.StructureGraphicsDef;
    }

    public void Setup(TilemapUpdater tilemapUpdater) { }

    void Start()
    {
        InitGraphics();
        ToBlueprintGraphics();
    }

    void InitGraphics()
    {
        _spriteRenderer.sprite = _spriteGraphicsDef.Sprite;
    }

    public void ToBlueprintGraphics()
    {
        _spriteRenderer.color = _spriteGraphicsDef.BlueprintColor;
    }

    public void ToNormalGraphics()
    {
        _spriteRenderer.color = Color.white;
    }
}
