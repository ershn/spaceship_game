using UnityEngine;
using UnityEngine.Events;

public class StructureStatus : MonoBehaviour
{
    public UnityEvent<float> OnSetupProgressed;

    StructureDef _structureDef;

    float _setupProgress;

    void Awake()
    {
        _structureDef = GetComponent<StructureDefHolder>().StructureDef;
    }

    void Start()
    {
        _setupProgress = _structureDef.SetupRequired ? 0f : 1f;
    }

    public void SetSetupProgress(float progress)
    {
        _setupProgress = progress;
        OnSetupProgressed.Invoke(_setupProgress);
    }
}
