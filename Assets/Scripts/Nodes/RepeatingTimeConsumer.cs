using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Consumers")]
public class RepeatingTimeConsumer : ExecuteConditionally<float>
{
    [Serialize, Inspectable, UnitHeaderInspectable("TimeInterval")]
    public float TimeInterval;

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
        consumedTime += Time.deltaTime;

        _progress = Mathf.Clamp01(consumedTime / TimeInterval);

        if (consumedTime >= TimeInterval)
        {
            consumedTime -= TimeInterval;
            return true;
        }
        else
            return false;
    }
}
