using UnityEngine;

[RequireComponent(typeof(BuildingComponents))]
public class BuildingLifecycle : MonoBehaviour
{
    BuildingConstructor _constructor;
    BuildingComponents _components;

    void Awake()
    {
        _constructor = GetComponent<BuildingConstructor>();
        _components = GetComponent<BuildingComponents>();
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