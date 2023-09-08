using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedDictionary<TKey, TValue>
    : Dictionary<TKey, TValue>,
        ISerializationCallbackReceiver
{
    [Serializable]
    struct Pair
    {
        public TKey Key;
        public TValue Value;

        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    [SerializeField]
    List<Pair> _pairs = new();

    public void OnBeforeSerialize()
    {
        var keyComparer = EqualityComparer<TKey>.Default;

        foreach (var (key, value) in this)
        {
            var index = _pairs.FindIndex(pair => keyComparer.Equals(pair.Key, key));
            if (index == -1)
                _pairs.Add(new(key, value));
            else
                _pairs[index] = new(key, value);
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();

        for (int index = _pairs.Count - 1; index >= 0; index--)
        {
            var pair = _pairs[index];
            if (pair.Key != null)
                this[pair.Key] = pair.Value;
        }
    }
}
