using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Structure")]
public class StructureDef : ScriptableObject, IGizmoDef
{
    public TileBase BlueprintTile;
    public TileBase NormalTile;

    public Object GizmoAsset => NormalTile;

    public ItemDefAmount[] ComponentAmounts;

    public int MaxHealthPoints = 100;

    public float ConstructionTime = 10f;
    public float DeconstructionTimeMultiplier = .5f;
}
