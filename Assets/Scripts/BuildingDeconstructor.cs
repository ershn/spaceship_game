using UnityEngine;

[RequireComponent(typeof(BuildingLifecycle))]
[RequireComponent(typeof(DeconstructionWork))]
public class BuildingDeconstructor : MonoBehaviour
{
    public WorkRequestManager WorkRequestManager;

    BuildingLifecycle _lifecycle;
    DeconstructionWork _deconstructionWork;

    bool _allowed;
    bool _started;

    void Awake()
    {
        _lifecycle = GetComponent<BuildingLifecycle>();
        _deconstructionWork = GetComponent<DeconstructionWork>();
    }

    public void Allow()
    {
        _allowed = true;
    }

    public void Deconstruct()
    {
        if (!_allowed || _started)
            return;

        _started = true;
        _deconstructionWork.OnWorkCompleted.AddListener(Complete);
        WorkRequestManager.RequestWork(_deconstructionWork);
    }

    void Complete()
    {
        _deconstructionWork.OnWorkCompleted.RemoveListener(Complete);
        _lifecycle.Destroy();
    }

    public void Cancel()
    {
        if (!_allowed || !_started)
            return;

        _deconstructionWork.OnWorkCompleted.RemoveListener(Complete);
        WorkRequestManager.CancelWork(_deconstructionWork);
        _deconstructionWork.ResetWorkTime();
        _started = false;
    }
}