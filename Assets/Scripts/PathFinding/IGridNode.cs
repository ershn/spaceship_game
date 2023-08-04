using UnityEngine;

namespace PathFinding
{
    public interface IGridNode
    {
        Vector2Int Position { get; set; }
        bool Walkable { get; set; }
    }
}
