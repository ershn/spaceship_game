using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PathFinding
{
    public class AStarPathFinder
    {
        readonly GridGraph<AStarGridNode> _grid;
        Guid _traversalId;

        public AStarPathFinder(GridGraph<AStarGridNode> grid)
        {
            _grid = grid;
        }

        uint GCost(AStarGridNode node) => node.GCost(_traversalId);

        void SetGCost(AStarGridNode node, uint cost) => node.SetGCost(_traversalId, cost);

        uint HCost(AStarGridNode node) => node.HCost(_traversalId);

        void SetHCost(AStarGridNode node, uint cost) => node.SetHCost(_traversalId, cost);

        uint FCost(AStarGridNode node) => node.FCost(_traversalId);

        AStarGridNode Parent(AStarGridNode node) => node.Parent(_traversalId);

        void SetParent(AStarGridNode node, AStarGridNode parent) =>
            node.SetParent(_traversalId, parent);

        public Vector2[] FindPath(Vector2 startPos, Vector2 endPos)
        {
            _traversalId = Guid.NewGuid();

            var startNode = _grid.World2DToNode(startPos);
            var endNode = _grid.World2DToNode(endPos);
            if (!(startNode.Walkable && endNode.Walkable))
                return null;

            var openSet = new Heap<AStarGridNode>(_grid.NodeCount);
            var closedSet = new HashSet<AStarGridNode>();

            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                var currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == endNode)
                    return RetracePath(endNode);

                foreach (var neighborNode in _grid.NodeNeighbors(currentNode))
                {
                    if (!neighborNode.Walkable || closedSet.Contains(neighborNode))
                        continue;

                    var newNeighborGCost =
                        GCost(currentNode) + DistanceCost(currentNode, neighborNode);
                    var neighborNodeInOpenSet = openSet.Contains(neighborNode);
                    if (newNeighborGCost < GCost(neighborNode) || !neighborNodeInOpenSet)
                    {
                        SetGCost(neighborNode, newNeighborGCost);
                        SetHCost(neighborNode, DistanceCost(neighborNode, endNode));
                        SetParent(neighborNode, currentNode);

                        if (!neighborNodeInOpenSet)
                            openSet.Add(neighborNode);
                        else
                            openSet.Update(neighborNode);
                    }
                }
            }
            return null;
        }

        uint DistanceCost(AStarGridNode nodeA, AStarGridNode nodeB)
        {
            var distanceX = Math.Abs(nodeB.Position.x - nodeA.Position.x);
            var distanceY = Math.Abs(nodeB.Position.y - nodeA.Position.y);

            if (distanceX > distanceY)
                return (uint)(distanceY * 14 + (distanceX - distanceY) * 10);
            else
                return (uint)(distanceX * 14 + (distanceY - distanceX) * 10);
        }

        Vector2[] RetracePath(AStarGridNode endNode)
        {
            var path = new List<AStarGridNode>();
            while (endNode != null)
            {
                path.Add(endNode);
                endNode = Parent(endNode);
            }

            var simplifiedPath = new List<AStarGridNode> { path[0] };
            var oldDirection = Vector2Int.zero;
            for (int i = 1; i < path.Count; i++)
            {
                var newDirection = path[i].Position - path[i - 1].Position;
                if (newDirection == oldDirection)
                    simplifiedPath[^1] = path[i];
                else
                    simplifiedPath.Add(path[i]);
                oldDirection = newDirection;
            }

            return simplifiedPath.Select(node => _grid.NodeToWorld2D(node)).Reverse().ToArray();
        }
    }
}
