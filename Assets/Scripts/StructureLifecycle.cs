using UnityEngine;

[RequireComponent(typeof(StructureComponents))]
public class StructureLifecycle : MonoBehaviour
{
    StructureConstructor _constructor;
    StructureComponents _components;

    void Awake()
    {
        _constructor = GetComponent<StructureConstructor>();
        _components = GetComponent<StructureComponents>();
    }

    void Start()
    {
        _constructor.Construct();
    }

    public void Destroy()
    {
        _components.Dump();
        Destroy(gameObject);
    }
}
