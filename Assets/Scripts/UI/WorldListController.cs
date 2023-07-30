using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldListController : MonoBehaviour
{
    [SerializeField]
    WorldEntryController _worldEntryPrefab;

    [SerializeField]
    WorldIndex _worldIndex;

    [SerializeField]
    UIActionReducer _actionReducer;

    readonly List<WorldEntryController> _worldEntries = new();

    void Awake()
    {
        InitializeWorldEntries();

        _worldIndex.OnChanged += UpdateWorldEntries;
    }

    void InitializeWorldEntries()
    {
        GetComponentsInChildren(_worldEntries);
        foreach (var entry in _worldEntries)
            entry.OnWorldSelected += _actionReducer.SelectWorld;
    }

    void UpdateWorldEntries(IEnumerable<World> worlds)
    {
        if (_worldEntries.Any(entry => entry == null)) // Is the scene being unloaded ?
            return;

        var worldCount = worlds.Count();
        var entryCount = _worldEntries.Count;

        for (; entryCount < worldCount; entryCount++)
            AddWorldEntry();
        for (; entryCount > worldCount; entryCount--)
            RemoveWorldEntry();

        foreach (var (world, entry) in worlds.ZipTuple(_worldEntries))
            entry.SetWorld(world);
    }

    void AddWorldEntry()
    {
        var entry = Instantiate(_worldEntryPrefab, transform);
        entry.OnWorldSelected += _actionReducer.SelectWorld;
        _worldEntries.Add(entry);
    }

    void RemoveWorldEntry()
    {
        var entry = _worldEntries[^1];
        _worldEntries.RemoveAt(_worldEntries.Count - 1);
        Destroy(entry.gameObject);
    }
}
