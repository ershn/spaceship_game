using static FunctionalUtils;
using UnityEngine;

public class StructureDeconstructor : MonoBehaviour
{
    TaskScheduler _taskScheduler;
    Destructor _destructor;
    DeconstructionWork _deconstructionWork;
    Canceler _canceler;

    [SerializeField]
    bool _allowed;
    bool _started;
    Task _task;

    void Awake()
    {
        _taskScheduler = GetComponentInParent<WorldInternalIO>().TaskScheduler;
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
        _taskScheduler.QueueTask(_task);
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
