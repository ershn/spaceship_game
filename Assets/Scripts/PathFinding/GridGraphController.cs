using UnityEngine;

namespace PathFinding
{
    public class GridGraphController : MonoBehaviour
    {
        [SerializeField]
        Vector2 _worldSize;

        [SerializeField]
        float _nodeSize;

        [SerializeField]
        float _circleColliderDiameter;

        [SerializeField]
        LayerMask _obstacleLayers;

        public GridGraph<AStarGridNode> Graph { get; private set; }

        void Awake()
        {
            Graph = GenerateGraph();
        }

        GridGraph<AStarGridNode> GenerateGraph()
        {
            var grid = new GridGraph<AStarGridNode>(transform.position, _worldSize, _nodeSize);
            grid.CreateNodes(new CircleCollisionChecker(_circleColliderDiameter, _obstacleLayers));
            return grid;
        }

        [SerializeField]
        bool _generateInEditor;

        void OnDrawGizmos()
        {
            if (_generateInEditor && Graph == null)
                Graph = GenerateGraph();
            else if (!_generateInEditor && Graph != null)
                Graph = null;

            if (Graph == null)
                return;

            foreach (var node in Graph.Nodes)
            {
                Gizmos.color = node.Walkable ? new Color(0, 1, 0, .4f) : new Color(1, 0, 0, .4f);
                Gizmos.DrawCube(
                    Graph.NodeToWorld2D(node),
                    new Vector2(Graph.NodeSize - .01f, Graph.NodeSize - .01f)
                );
            }
        }
    }
}
