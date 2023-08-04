using UnityEngine;

namespace PathFinding
{
    public interface ICollisionChecker
    {
        bool CollisionFound(Vector2 position);
    }
}
