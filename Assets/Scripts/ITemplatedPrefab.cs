using UnityEngine;

public interface ITemplatedPrefab
{
    GameObject Template { get; }
    GameObject Prefab { set; }
}
