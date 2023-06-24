using System;
using UnityEngine;

public class Timer
{
    public static Timer Delay(float delay, Action action) => new(delay, action, repeat: false);

    public static Timer Repeat(float interval, Action action) =>
        new(interval, action, repeat: true);

    float _delay;
    Action _action;
    bool _repeat;

    float? _startTime;

    public Timer(float delay, Action action, bool repeat)
    {
        _delay = delay;
        _action = action;
        _repeat = repeat;
    }

    public bool Completed { get; private set; }

    public void Run()
    {
        if (Completed)
            return;

        if (_startTime is float startTime)
        {
            if (Time.time - startTime < _delay)
                return;

            _action();

            if (_repeat)
                _startTime = startTime + _delay;
            else
                Completed = true;
        }
        else
        {
            _startTime = Time.time;
        }
    }
}
