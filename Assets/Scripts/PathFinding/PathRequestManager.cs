using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    public class PathRequestManager : MonoBehaviour
    {
        [SerializeField]
        GridGraphController _gridGraphController;

        GridGraph<AStarGridNode> Graph => _gridGraphController.Graph;

        AStarPathFinder _pathFinder;
        AStarPathFinder PathFinder
        {
            get
            {
                _pathFinder ??= new(Graph);
                return _pathFinder;
            }
        }

        readonly Queue<PathRequest> _requests = new();

        public void RequestPath(PathRequest request)
        {
            _requests.Enqueue(request);
        }

        void Update()
        {
            PathRequest request;
            while (_requests.TryDequeue(out request) && request.Canceled)
                ;
            if (request == null)
                return;

            var path = PathFinder.FindPath(request.StartPosition, request.EndPosition);
            if (path != null)
                request.Complete(PathResponse.Succeeded(path));
            else
                request.Complete(PathResponse.Failed());
        }
    }
}
