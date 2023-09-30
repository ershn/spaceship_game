using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class StructureDeconstructor : MonoBehaviour
{
    JobScheduler _jobScheduler;
    Destructor _destructor;
    DeconstructionWork _deconstructionWork;
    Canceler _canceler;

    [SerializeField]
    bool _allowed;
    bool _started;

    void Awake()
    {
        _jobScheduler = GetComponentInParent<WorldInternalIO>().JobScheduler;
        _destructor = GetComponent<Destructor>();
        _deconstructionWork = GetComponent<DeconstructionWork>();
        _canceler = GetComponent<Canceler>();
    }

    public void Allow()
    {
        _allowed = true;
    }

    public async void Deconstruct()
    {
        if (!_allowed || _started)
            return;

        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var unregister = _canceler.OnCancel.Register(cts.Cancel);
        try
        {
            _started = true;
            var job = new WorkOnJob(_deconstructionWork);
            await _jobScheduler.Execute(job, ct);
            _destructor.Destroy();
        }
        catch (TaskCanceledException)
        {
            _started = false;
            _deconstructionWork.Reset();
            // throw; //? Should the exception be re-thrown ?
        }
        finally
        {
            unregister();
        }
    }
}
