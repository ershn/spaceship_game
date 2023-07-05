using System;
using UnityEngine;

[Serializable]
public class StructureSpriteGraphicsDef : StructureGraphicsDef
{
    public GameObject Prefab;

    public Sprite Sprite;
    public RuntimeAnimatorController AnimatorController;

    public override void Setup(GameObject gameObject) =>
        UnityEngine.Object.Instantiate(Prefab, gameObject.transform);

    public override UnityEngine.Object GizmoAsset => Sprite;
}
