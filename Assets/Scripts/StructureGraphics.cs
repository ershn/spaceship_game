using UnityEngine;

public class StructureGraphics : MonoBehaviour
{
    public TilemapUpdater TilemapUpdater;

    IStructureGraphics _graphics;

    void Awake()
    {
        var graphicsDef = GetComponent<StructureDefHolder>().StructureDef.StructureGraphicsDef;
        _graphics = (IStructureGraphics)gameObject.AddComponent(graphicsDef.RendererType);
        _graphics.Setup(TilemapUpdater);
    }

    public void ToBlueprintGraphics() => _graphics.ToBlueprintGraphics();

    public void ToNormalGraphics() => _graphics.ToNormalGraphics();
}
