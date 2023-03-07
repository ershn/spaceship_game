using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GridIndexer : ScriptableObject
{
    Dictionary<Vector2Int, GameObject> _positionToGameObject = new();

    public GameObject Get(Vector2Int position) => _positionToGameObject[position];

    public bool Has(Vector2Int position) => _positionToGameObject.ContainsKey(position);

    public void Add(Vector2Int position, GameObject gameObject)
    {
        _positionToGameObject[position] = gameObject;

        OnAdd(position, gameObject);
    }

    protected virtual void OnAdd(Vector2Int position, GameObject gameObject) { }

    public void Remove(Vector2Int position)
    {
        var gameObject = _positionToGameObject[position];
        _positionToGameObject.Remove(position);

        OnRemove(position, gameObject);
    }

    protected virtual void OnRemove(Vector2Int position, GameObject gameObject) { }
}