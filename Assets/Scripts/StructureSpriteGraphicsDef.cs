using System;
using UnityEngine;

[Serializable]
public class StructureSpriteGraphicsDef : StructureGraphicsDef
{
    public override void Template(GameObject gameObject)
    {
        var prefab = UnityEngine.Object.Instantiate(Prefab, gameObject.transform);
        Templater.Template(prefab, this);
    }

    public GameObject Prefab;

    public Sprite Sprite;
    public Color BlueprintColor;
    public RuntimeAnimatorController AnimatorController;
}
