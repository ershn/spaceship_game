using UnityEngine;

public class StructureSpriteGraphics : MonoBehaviour, ITemplate<StructureSpriteGraphicsDef>
{
    public void Template(StructureSpriteGraphicsDef def)
    {
        GetComponent<SpriteRenderer>().sprite = def.Sprite;
        GetComponent<Animator>().runtimeAnimatorController = def.AnimatorController;
    }

    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();

        var structureGraphics = transform.parent.GetComponent<StructureGraphics>();
        structureGraphics.OnConstructionCompleted += OnConstructionCompleted;
        structureGraphics.OnSetupProgressed += OnSetupProgressed;
    }

    void OnConstructionCompleted() => _animator.SetTrigger("ConstructionCompleted");

    void OnSetupProgressed(float progress) => _animator.SetFloat("SetupProgress", progress);
}
