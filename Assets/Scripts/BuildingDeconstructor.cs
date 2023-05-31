using UnityEngine;

[RequireComponent(typeof(BuildingLifecycle))]
[RequireComponent(typeof(DeconstructionWork))]
public class BuildingDeconstructor : MonoBehaviour
{
    public TaskScheduler TaskScheduler;

    BuildingLifecycle _lifecycle;
    DeconstructionWork _deconstructionWork;

    bool _allowed;
    bool _started;
    ITask _task;

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
        _task = TaskCreator.WorkOn(_deconstructionWork);
        TaskScheduler.QueueTask(_task);
    }

    void Complete()
    {
        _deconstructionWork.OnWorkCompleted.RemoveListener(Complete);
        _lifecycle.Destroy();
    }

    public void Cancel()
    {
        if (!_started)
            return;

        _deconstructionWork.OnWorkCompleted.RemoveListener(Complete);
        _task.Cancel();
        _deconstructionWork.Reset();
        _task = null;
        _started = false;
    }
}
