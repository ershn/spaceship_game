using System.Collections.Generic;
using UnityEngine;

public class GridIndexes : MonoBehaviour
{
    [SerializeField]
    Grid _grid;
    public Grid Grid => _grid;

    public GOListGridIndex GlobalGrid { get; }
    public ItemGridIndex ItemGrid { get; }
    public GOGridIndex FloorGrid { get; }
    public GOGridIndex FurnitureGrid { get; }

    public GridIndexes()
    {
        GlobalGrid = new();
        ItemGrid = new(GlobalGrid);
        FloorGrid = new(GlobalGrid);
        FurnitureGrid = new(GlobalGrid);
    }

    public IGridIndex GetLayerIndex(WorldLayer worldLayer) =>
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
