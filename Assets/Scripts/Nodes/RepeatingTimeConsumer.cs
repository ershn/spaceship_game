using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Consumers")]
public class RepeatingTimeConsumer : ExecuteConditionally<float>
{
    [Serialize, Inspectable, UnitHeaderInspectable("TimeInterval")]
    public float TimeInterval;

    protected override float Init(GameObject gameObject) => default;

    protected override bool Execute(ref float consumedTime)
    {
        consumedTime += Time.deltaTime;

        if (consumedTime >= TimeInterval)
        {
            consumedTime -= TimeInterval;
            return true;
        }
        else
            return false;
    }
}
