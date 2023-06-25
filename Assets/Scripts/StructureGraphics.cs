using UnityEngine;

public class StructureGraphics : MonoBehaviour
{
    IStructureGraphics _graphics;

    void Awake()
    {
        var graphicsDef = GetComponent<StructureDefHolder>().StructureDef.StructureGraphicsDef;
        _graphics = (IStructureGraphics)gameObject.AddComponent(graphicsDef.RendererType);
    }

    public void ToBlueprintGraphics() => _graphics.ToBlueprintGraphics();

    public void ToNormalGraphics() => _graphics.ToNormalGraphics();
}
