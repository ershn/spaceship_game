using UnityEngine;

namespace PathFinding
{
    public class CircleCollisionChecker : ICollisionChecker
    {
        float _circleDiameter;
        LayerMask _obstacleLayers;

        public CircleCollisionChecker(float circleDiameter, LayerMask obstacleLayers)
        {
            _circleDiameter = circleDiameter;
            _obstacleLayers = obstacleLayers;
        }

        public bool CollisionFound(Vector2 position)
        {
            var collision = Physics2D.OverlapCircle(
                position,
                _circleDiameter / 2f,
                _obstacleLayers
            );
            return collision != null;
        }
    }
}
