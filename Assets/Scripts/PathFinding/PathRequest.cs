using System;
using UnityEngine;

namespace PathFinding
{
    public class PathRequest
    {
        public event Action<PathResponse> OnCompleted;

        public readonly Vector2 StartPosition;
        public readonly Vector2 EndPosition;

        public bool Completed => Response != null;
        public bool Canceled { get; private set; }

        public PathResponse Response { get; private set; }

        public PathRequest(Vector2 startPos, Vector2 endPos)
        {
            StartPosition = startPos;
            EndPosition = endPos;
        }

        public void Complete(PathResponse response)
        {
            Response = response;
            OnCompleted?.Invoke(Response);
        }

        public void Cancel()
        {
            if (Completed)
                return;

            Canceled = true;
            Response = PathResponse.Failed();
            OnCompleted?.Invoke(Response);
        }
    }
}
