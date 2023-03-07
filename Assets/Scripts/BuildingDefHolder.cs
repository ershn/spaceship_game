using UnityEngine;

public class BuildingDefHolder : MonoBehaviour, IHealthDef
{
    public BuildingDef BuildingDef;

    public int MaxHealthPoints { get => BuildingDef.MaxHealthPoints; }
}