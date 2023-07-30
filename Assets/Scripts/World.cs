using UnityEngine;

public class World : MonoBehaviour
{
    WorldIndex _worldIndex;

    [SerializeField]
    uint _index;
    public uint Index => _index;

    void Awake()
    {
        _worldIndex = GetComponentInParent<WorldIndex>();
    }

    void Start()
    {
        _worldIndex.RegisterWorld(this);
    }

    void OnDestroy()
    {
        _worldIndex.UnregisterWorld(this);
    }
}
