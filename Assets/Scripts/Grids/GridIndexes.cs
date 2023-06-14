using System.Collections.Generic;
using UnityEngine;

public class GridIndexes : MonoBehaviour
{
    public ItemGridIndex ItemGrid { get; } = new();
    public GOGridIndex FloorGrid { get; } = new();
    public GOGridIndex FurnitureGrid { get; } = new();

    public GridIndex GetLayerIndex(WorldLayer worldLayer) =>
        worldLayer switch
        {
            WorldLayer.Item => ItemGrid,
            WorldLayer.Floor => FloorGrid,
            WorldLayer.Furniture => FurnitureGrid,
            _ => throw new System.NotImplementedException(),
        };

    public GOGridIndex GetStructureLayerIndex(WorldLayer structureLayer) =>
        structureLayer switch
        {
            WorldLayer.Floor => FloorGrid,
            WorldLayer.Furniture => FurnitureGrid,
            _ => throw new System.NotImplementedException(),
        };

    public IEnumerable<GOGridIndex> GetStructureLayerIndexes(WorldLayer structureLayers)
    {
        var indexes = new List<GOGridIndex>();
        if (structureLayers.HasFlag(WorldLayer.Floor))
            indexes.Add(FloorGrid);
        if (structureLayers.HasFlag(WorldLayer.Furniture))
            indexes.Add(FurnitureGrid);
        return indexes;
    }
}
