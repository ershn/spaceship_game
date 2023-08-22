using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class StructureTileGraphicsDef : StructureGraphicsDef
{
    public override void Template(GameObject gameObject)
    {
        var prefab = UnityEngine.Object.Instantiate(Prefab, gameObject.transform);
        Templater.Template(prefab, this);
    }

    public GameObject Prefab;

    public TileBase Tile;
    public Color BlueprintColor;
}
