using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    public class GridGraph<T>
        where T : IGridNode, new()
    {
        public readonly Vector2 Origin;
        public readonly Vector2Int Size;
        public readonly float NodeSize;
        public readonly T[,] Nodes;

        public GridGraph(Vector2 worldCenter, Vector2 worldSize, float nodeSize)
        {
            NodeSize = nodeSize;
            Size = Vector2Int.RoundToInt(worldSize / NodeSize);
            Origin = worldCenter - worldSize / 2f;
            Nodes = new T[Size.x, Size.y];
        }

        public int NodeCount => Size.x * Size.y;

        public void CreateNodes(ICollisionChecker collisionChecker)
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    var position = new Vector2Int(x, y);
                    var walkable = !collisionChecker.CollisionFound(GridToWorld2D(position));
                    Nodes[x, y] = new T() { Position = position, Walkable = walkable };
                }
            }
        }

        public Vector2 GridToWorld2D(Vector2Int position) =>
            Origin + (Vector2)position * NodeSize + Vector2.one * (NodeSize / 2f);

        public Vector2 NodeToWorld2D(T node) => GridToWorld2D(node.Position);

        public T World2DToNode(Vector2 position)
        {
            var gridPosition = Vector2Int.FloorToInt((position - Origin) / NodeSize);
            return Nodes[gridPosition.x, gridPosition.y];
        }

        public IEnumerable<T> NodeNeighbors(T node)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    var pos = node.Position + new Vector2Int(x, y);
                    if (pos.x >= 0 && pos.x < Size.x && pos.y >= 0 && pos.y < Size.y)
                        yield return Nodes[pos.x, pos.y];
                }
            }
        }
    }
}
