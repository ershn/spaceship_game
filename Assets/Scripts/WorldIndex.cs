using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldIndex : MonoBehaviour
{
    class WorldSorter : IComparer<World>
    {
        public int Compare(World x, World y) => x.Index.CompareTo(y.Index);
    }

    public event Action<IEnumerable<World>> OnChanged;

    readonly SortedSet<World> _worlds = new(new WorldSorter());

    public void RegisterWorld(World world)
    {
        _worlds.Add(world);
        OnChanged?.Invoke(_worlds);
    }

    public void UnregisterWorld(World world)
    {
        _worlds.Remove(world);
        OnChanged?.Invoke(_worlds);
    }
}
