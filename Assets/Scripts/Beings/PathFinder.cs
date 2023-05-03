using System;
using UnityEngine;
using Pathfinding;

public class PathFinder : MonoBehaviour
{
    public float MinDistanceForNextWaypoint = .5f;
    public float MinDistanceForStop = .2f;

    public Vector2Event OnMoveDirectionUpdated;

    Seeker _seeker;

    void Awake()
    {
        _seeker = GetComponent<Seeker>();
    }

#region executor

    bool _executing = false;
    Action<bool> _onEnd;
    bool _pathCalculationCanceled;

    bool IsExecuting() => _executing;

    void StartExecuting(Vector2 dest, Action<bool> onEnd)
    {
        _executing = true;
        _onEnd = onEnd;

        CalculatePath(dest);
    }

    void StopExecuting(bool success)
    {
        _executing = false;
        _onEnd(success);
    }

    void CalculatePath(Vector2 dest)
    {
        _pathCalculationCanceled = false;
        _seeker.StartPath(transform.position, dest, OnPathCalculated);
    }

    void CancelPathCalculation()
    {
        _pathCalculationCanceled = true;
        _seeker.CancelCurrentPathRequest();
        StopExecuting(success: false);
    }

    void OnPathCalculated(Path path)
    {
        if (_pathCalculationCanceled)
            return;

        if (path.error)
            StopExecuting(success: false);
        else
            StartMoving(path, StopExecuting);
    }

    public void MoveTo(Vector2 dest, Action<bool> onEnd)
    {
        if (IsExecuting())
            throw new InvalidOperationException("Already moving to a destination");

        StartExecuting(dest, onEnd);
    }

    public void Cancel()
    {
        if (!IsExecuting())
            throw new InvalidOperationException("Not moving to any destination");

        if (!IsMoving())
            CancelPathCalculation();
        else
            StopMoving(success: false);
    }

#endregion

#region mover

    Path _path;
    int _currentWaypointIndex;
    Action<bool> _onMovementEnd;

    bool IsMoving() => _path != null;

    void StartMoving(Path path, Action<bool> onMoveEnd)
    {
        _path = path;
        _currentWaypointIndex = 0;
        _onMovementEnd = onMoveEnd;
    }

    void StopMoving(bool success)
    {
        _path = null;
        OnMoveDirectionUpdated.Invoke(Vector2.zero);
        _onMovementEnd(success);
    }

    void Move()
    {
        var destDistance = Vector2.Distance(transform.position, _path.vectorPath[^1]);
        if (destDistance < MinDistanceForStop)
        {
            StopMoving(success: true);
            return;
        }

        var currentWaypoint = _path.vectorPath[_currentWaypointIndex];
        var moveDirection = (currentWaypoint - transform.position).normalized;
        OnMoveDirectionUpdated.Invoke(moveDirection);

        var waypointDistance = Vector3.Distance(transform.position, currentWaypoint);
        if (waypointDistance < MinDistanceForNextWaypoint &&
            _currentWaypointIndex < _path.vectorPath.Count - 1)
            _currentWaypointIndex++;
    }

    void FixedUpdate()
    {
        if (IsMoving())
            Move();
    }

#endregion
}