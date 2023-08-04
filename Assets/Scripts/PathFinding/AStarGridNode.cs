using System;
using UnityEngine;

namespace PathFinding
{
    public class AStarGridNode : IGridNode, IHeapItem<AStarGridNode>
    {
        public Vector2Int Position { get; set; }
        public bool Walkable { get; set; }

        public int HeapIndex { get; set; }

        public int CompareTo(AStarGridNode other)
        {
            var compare = FCost().CompareTo(other.FCost());
            if (compare == 0)
                compare = _hCost.CompareTo(other._hCost);
            return compare;
        }

        Guid _traversalId;
        uint _gCost;
        uint _hCost;
        AStarGridNode _parent;

        uint FCost() => _gCost + _hCost;

        public uint GCost(Guid traversalId) => ForTraversal(traversalId)._gCost;

        public void SetGCost(Guid traversalId, uint cost) =>
            ForTraversal(traversalId)._gCost = cost;

        public uint HCost(Guid traversalId) => ForTraversal(traversalId)._hCost;

        public void SetHCost(Guid traversalId, uint cost) =>
            ForTraversal(traversalId)._hCost = cost;

        public uint FCost(Guid traversalId) => ForTraversal(traversalId).FCost();

        public AStarGridNode Parent(Guid traversalId) => ForTraversal(traversalId)._parent;

        public void SetParent(Guid traversalId, AStarGridNode parent) =>
            ForTraversal(traversalId)._parent = parent;

        AStarGridNode ForTraversal(Guid traversalId)
        {
            if (_traversalId != traversalId)
            {
                _traversalId = traversalId;
                _gCost = 0;
                _hCost = 0;
                _parent = null;
            }
            return this;
        }
    }
}
