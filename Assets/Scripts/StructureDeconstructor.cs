using static FunctionalUtils;
using UnityEngine;

[RequireComponent(typeof(DeconstructionWork))]
[RequireComponent(typeof(StructureCanceler))]
[RequireComponent(typeof(StructureLifecycle))]
public class StructureDeconstructor : MonoBehaviour
{
    public TaskScheduler TaskScheduler;

    StructureLifecycle _lifecycle;
    DeconstructionWork _deconstructionWork;
    StructureCanceler _structureCanceler;

    bool _allowed;
    bool _started;
    ITask _task;

    void Awake()
    {
        _lifecycle = GetComponent<StructureLifecycle>();
        _deconstructionWork = GetComponent<DeconstructionWork>();
        _structureCanceler = GetComponent<StructureCanceler>();
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

        var unregister = _structureCanceler.OnCancel.Register(Cancel);
        _task = TaskCreator.WorkOn(_deconstructionWork);
        _task.Then(Do<bool>(unregister, Complete));
        TaskScheduler.QueueTask(_task);
    }

    void Complete(bool succeeded)
    {
        if (succeeded)
            _lifecycle.Destroy();
        else
            _deconstructionWork.Reset();
    }

    public void Cancel()
    {
        if (!_started)
            return;
        _started = false;

        _task.Cancel();
        _task = null;
    }
}
