using UnityEngine;

public class StructureSpriteGraphics : MonoBehaviour
{
    StructureSpriteGraphicsDef _spriteGraphicsDef;

    StructureGraphics _structureGraphics;
    SpriteRenderer _spriteRenderer;
    Animator _animator;

    void Awake()
    {
        var parent = transform.parent;
        name = $"{parent.name}Graphics";

        var structureDef = parent.GetComponent<StructureDefHolder>().StructureDef;
        _spriteGraphicsDef = (StructureSpriteGraphicsDef)structureDef.StructureGraphicsDef;

        _structureGraphics = parent.GetComponent<StructureGraphics>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        Setup();
    }

    void Setup()
    {
        _spriteRenderer.sprite = _spriteGraphicsDef.Sprite;
        _animator.runtimeAnimatorController = _spriteGraphicsDef.AnimatorController;

        _structureGraphics.OnConstructionCompleted += OnConstructionCompleted;
        _structureGraphics.OnSetupProgressed += OnSetupProgressed;
    }

    void OnConstructionCompleted() => _animator.SetTrigger("ConstructionCompleted");

    void OnSetupProgressed(float progress) => _animator.SetFloat("SetupProgress", progress);
}
