using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PathFinder : MonoBehaviour
{
    public float MinDistanceForNextWaypoint = .5f;
    public float MinDistanceForStop = .2f;

    public Vector2Event OnMoveDirectionUpdated;

    PathSeeker _seeker;

    void Awake()
    {
        _seeker = GetComponent<PathSeeker>();
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
        _seeker.RequestPath(transform.position, dest, OnPathCalculated);
    }

    void CancelPathCalculation()
    {
        _pathCalculationCanceled = true;
        _seeker.CancelCurrentPathRequest();
        StopExecuting(success: false);
    }

    void OnPathCalculated(Vector2[] path)
    {
        if (_pathCalculationCanceled)
            return;

        if (path == null)
            StopExecuting(success: false);
        else
            StartMoving(path, StopExecuting);
    }

    public void MoveTo(Vector2 dest, Action<bool> onEnd)
    {
        Assert.IsFalse(IsExecuting());

        StartExecuting(dest, onEnd);
    }

    public void Cancel()
    {
        Assert.IsTrue(IsExecuting());

        if (!IsMoving())
            CancelPathCalculation();
        else
            StopMoving(success: false);
    }

    #endregion

    #region mover

    Vector2[] _path;
    int _currentWaypointIndex;
    Action<bool> _onMovementEnd;

    bool IsMoving() => _path != null;

    void StartMoving(Vector2[] path, Action<bool> onMoveEnd)
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
        var destDistance = Vector2.Distance(transform.position, _path[^1]);
        if (destDistance < MinDistanceForStop)
        {
            StopMoving(success: true);
            return;
        }

        var currentWaypoint = _path[_currentWaypointIndex];
        var moveDirection = (currentWaypoint - (Vector2)transform.position).normalized;
        OnMoveDirectionUpdated.Invoke(moveDirection);

        var waypointDistance = Vector2.Distance(transform.position, currentWaypoint);
        if (
            waypointDistance < MinDistanceForNextWaypoint
            && _currentWaypointIndex < _path.Length - 1
        )
            _currentWaypointIndex++;
    }

    void FixedUpdate()
    {
        if (IsMoving())
            Move();
    }

    #endregion
}
