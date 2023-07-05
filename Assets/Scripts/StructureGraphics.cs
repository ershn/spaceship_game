using System;
using UnityEngine;

public class StructureGraphics : MonoBehaviour
{
    public event Action OnConstructionCompleted;
    public event Action<float> OnSetupProgressed;

    void Awake()
    {
        var structureDef = GetComponent<StructureDefHolder>().StructureDef;
        structureDef.StructureGraphicsDef.Setup(gameObject);
    }

    public void ConstructionCompleted() => OnConstructionCompleted?.Invoke();

    public void SetupProgressed(float progress) => OnSetupProgressed?.Invoke(progress);
}
