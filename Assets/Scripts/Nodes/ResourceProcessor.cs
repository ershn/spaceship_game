using Unity.VisualScripting;
using UnityEngine;

public abstract class ResourceProcessor : Unit, IGraphElementWithData
{
    public abstract class Instance : IGraphElementData
    {
        public GameObject GameObject;

        public virtual float Progress => Completed ? 1f : 0f;

        public virtual bool Completed => Progress >= 1f;

        public virtual void OnStart() { }

        public abstract void OnUpdate();

        public virtual void OnReset() { }
    }

    [Serialize, Inspectable, UnitHeaderInspectable("Reset on Completed")]
    public bool ResetOnCompleted = true;

    [DoNotSerialize]
    public ControlInput Start;

    [DoNotSerialize, PortLabelHidden]
    public ControlOutput OnStarted;

    [DoNotSerialize]
    public ControlInput Update;

    [DoNotSerialize, PortLabelHidden]
    public ControlOutput OnUpdated;

    [DoNotSerialize]
    public ControlOutput OnCompleted;

    [DoNotSerialize]
    public ValueOutput Progress;

    [DoNotSerialize]
    public ValueOutput Completed;

    [DoNotSerialize]
    public ControlInput Reset;

    protected override void Definition()
    {
        Start = ControlInput(nameof(Start), ControlStart);
        OnStarted = ControlOutput(nameof(OnStarted));
        Succession(Start, OnStarted);

        Update = ControlInput(nameof(Update), ControlUpdate);
        OnUpdated = ControlOutput(nameof(OnUpdated));
        Succession(Update, OnUpdated);
        OnCompleted = ControlOutput(nameof(OnCompleted));
        Succession(Update, OnCompleted);
        Progress = ValueOutput(nameof(Progress), flow => GetInstance(flow).Progress);
        Assignment(Update, Progress);
        Completed = ValueOutput(nameof(Completed), flow => GetInstance(flow).Completed);
        Assignment(Update, Completed);

        Reset = ControlInput(nameof(Reset), ControlReset);
    }

    ControlOutput ControlStart(Flow flow)
    {
        var instance = GetInstance(flow);
        instance.GameObject = flow.stack.gameObject;
        instance.OnStart();
        return OnStarted;
    }

    ControlOutput ControlUpdate(Flow flow)
    {
        var instance = GetInstance(flow);
        if (ResetOnCompleted && instance.Completed)
            instance.OnReset();
        if (!instance.Completed)
            instance.OnUpdate();
        if (instance.Completed)
            flow.Invoke(OnCompleted);
        return OnUpdated;
    }

    ControlOutput ControlReset(Flow flow)
    {
        var instance = GetInstance(flow);
        instance.OnReset();
        return null;
    }

    public IGraphElementData CreateData() => Instantiate();

    protected abstract Instance Instantiate();

    Instance GetInstance(Flow flow) => flow.stack.GetElementData<Instance>(this);
}
