using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Consumers")]
public class TimeConsumer : ExecuteConditionally<float>
{
    [Serialize, Inspectable, UnitHeaderInspectable("TotalTime")]
    public float TotalTime;

    protected override float Init(GameObject gameObject) => default;

    protected override bool Execute(ref float consumedTime)
    {
        if (consumedTime < TotalTime)
        {
            consumedTime += Time.deltaTime;
            return consumedTime >= TotalTime;
        }
        else
            return true;
    }
}
