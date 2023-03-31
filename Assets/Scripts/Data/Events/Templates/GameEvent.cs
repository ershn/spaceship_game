using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/()")]
public class GameEvent : ScriptableObject
{
    List<GameEventListener> _listeners = new();

    public void AddListener(GameEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(GameEventListener listener)
    {
        _listeners.Remove(listener);
    }

    public void Invoke()
    {
        _listeners.ForEach(listener => listener.OnEventInvoked());
    }
}
