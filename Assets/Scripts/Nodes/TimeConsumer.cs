using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Consumers")]
public class TimeConsumer : ExecuteConditionally<float>
{
    [Serialize, Inspectable, UnitHeaderInspectable("TotalTime")]
    public float TotalTime;

    [DoNotSerialize]
    public ValueOutput Progress;

    float _progress;

    protected override void Definition()
    {
        base.Definition();

        Progress = ValueOutput("Progress", _ => _progress);

        Assignment(Enter, Progress);
    }

    protected override float Init(GameObject gameObject) => default;

    protected override bool Execute(ref float consumedTime)
    {
        if (consumedTime < TotalTime)
        {
            consumedTime += Time.deltaTime;
            _progress = Mathf.Clamp01(consumedTime / TotalTime);
            return consumedTime >= TotalTime;
        }
        else
        {
            _progress = 1f;
            return true;
        }
    }
}
