using UnityEngine;

namespace PathFinding
{
    public class PathResponse
    {
        public static PathResponse Succeeded(Vector2[] path) => new(true, path);

        public static PathResponse Failed() => new(false, null);

        public readonly bool Success;
        public readonly Vector2[] Path;

        PathResponse(bool success, Vector2[] path)
        {
            Success = success;
            Path = path;
        }
    }
}
