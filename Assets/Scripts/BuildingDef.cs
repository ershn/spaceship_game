using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Building/Generic")]
public class BuildingDef : ScriptableObject, IGizmoDef
{
    public TileBase BlueprintTile;
    public TileBase NormalTile;

    public Object GizmoAsset => NormalTile;

    public ItemDefMass[] ComponentMasses;

    public int MaxHealthPoints = 100;

    public float ConstructionTime = 10f;
    public float DeconstructionTimeMultiplier = .5f;
}
