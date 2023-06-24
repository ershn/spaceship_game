using static FunctionalUtils;
using UnityEngine;

[RequireComponent(typeof(DeconstructionWork))]
[RequireComponent(typeof(Canceler))]
public class StructureDeconstructor : MonoBehaviour
{
    public TaskScheduler TaskScheduler;

    Destructor _destructor;
    DeconstructionWork _deconstructionWork;
    Canceler _canceler;

    bool _allowed;
    bool _started;
    ITask _task;

    void Awake()
    {
        _destructor = GetComponent<Destructor>();
        _deconstructionWork = GetComponent<DeconstructionWork>();
        _canceler = GetComponent<Canceler>();
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

        var unregister = _canceler.OnCancel.Register(Cancel);
        _task = TaskCreator.WorkOn(_deconstructionWork);
        _task.Then(Do<bool>(unregister, Complete));
        TaskScheduler.QueueTask(_task);
    }

    void Complete(bool succeeded)
    {
        if (succeeded)
            _destructor.Destroy();
        else
            _deconstructionWork.Reset();
    }

    void Cancel()
    {
        if (!_started)
            return;
        _started = false;

        _task.Cancel();
        _task = null;
    }
}
