using UnityEngine;

public class GridIndexes : MonoBehaviour
{
    public ItemGridIndex ItemGrid { get; } = new();
    public GOGridIndex FloorGrid { get; } = new();
    public GOGridIndex FurnitureGrid { get; } = new();

    public GridIndex GetIndex(GridIndexType type) =>
        type switch
        {
            GridIndexType.ItemGrid => ItemGrid,
            GridIndexType.FloorGrid => FloorGrid,
            GridIndexType.FurnitureGrid => FurnitureGrid,
            _ => throw new System.NotImplementedException(),
        };
}
