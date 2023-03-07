using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class BuildingDef : ScriptableObject
{
    public TileBase BlueprintTile;
    public TileBase NormalTile;

    public ItemDefAmount[] ComponentAmounts;

    public int MaxHealthPoints = 100;

    [Obsolete]
    public float BuildingUnitTime = .03f;

    public float ConstructionTime = 20f;
}
