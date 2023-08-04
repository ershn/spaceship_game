using System;
using UnityEngine;

namespace PathFinding
{
    public class PathSeeker : MonoBehaviour
    {
        public PathRequestManager PathRequestManager;

        PathRequest _currentRequest;

        public PathRequest RequestPath(Vector2 startPos, Vector2 endPos, Action<Vector2[]> callback)
        {
            _currentRequest?.Cancel();
            _currentRequest = new PathRequest(startPos, endPos);
            _currentRequest.OnCompleted += response => callback(response.Path);
            PathRequestManager.RequestPath(_currentRequest);
            return _currentRequest;
        }

        public void CancelCurrentPathRequest()
        {
            _currentRequest?.Cancel();
        }
    }
}
