using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Index<TKey, TValue>
    where TKey : IEquatable<TKey>
{
    [Serializable]
    struct Entry
    {
        public TKey Key;
        public TValue Value;
    }

    [SerializeField]
    Entry[] _index = Array.Empty<Entry>();

    public TValue this[TKey key]
    {
        get { return _index.First(entry => entry.Key.Equals(key)).Value; }
    }

    public IEnumerable<TKey> Keys => _index.Select(entry => entry.Key);
    public IEnumerable<TValue> Values => _index.Select(entry => entry.Value);
}
