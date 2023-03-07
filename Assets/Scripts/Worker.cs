using System;
using UnityEngine;

public class Worker : MonoBehaviour
{
    IWork _work;
    Action<bool> _onEnd;

    public void WorkOn(IWork work, Action<bool> onEnd)
    {
        if (_work != null)
            throw new InvalidOperationException("Already working.");

        _work = work;
        _onEnd = onEnd;
    }

    public void Cancel()
    {
        if (_work == null)
            throw new InvalidOperationException("Not working.");

        _work = null;
        _onEnd(false);
    }

    void Update()
    {
        if (_work == null)
            return;

        var finished = _work.AddWorkTime(Time.deltaTime);
        if (finished)
        {
            _work = null;
            _onEnd(true);
        }
    }
}