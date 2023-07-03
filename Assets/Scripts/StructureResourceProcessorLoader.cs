using Unity.VisualScripting;
using UnityEngine;

public class StructureResourceProcessorLoader : MonoBehaviour
{
    StructureDef _structureDef;

    void Awake()
    {
        _structureDef = GetComponent<StructureDefHolder>().StructureDef;
    }

    void Start()
    {
        if (_structureDef.ResourceProcessor != null)
            Load(_structureDef.ResourceProcessor);
        Destroy(this);
    }

    void Load(StateGraphAsset resourceProcessor)
    {
        var stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.nest.SwitchToMacro(resourceProcessor);
    }
}
