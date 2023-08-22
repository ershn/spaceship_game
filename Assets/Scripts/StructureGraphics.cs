using System;
using UnityEngine;

public class StructureGraphics : MonoBehaviour, ITemplate<StructureDef>
{
    public void Template(StructureDef structureDef)
    {
        structureDef.StructureGraphicsDef.Template(gameObject);
    }

    public event Action OnConstructionCompleted;
    public event Action<float> OnSetupProgressed;

    public void ConstructionCompleted() => OnConstructionCompleted?.Invoke();

    public void SetupProgressed(float progress) => OnSetupProgressed?.Invoke(progress);
}
