using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Consumers")]
public class TimeConsumer : ResourceProcessor
{
    [Serialize, Inspectable, UnitHeaderInspectable("Time")]
    public float TimeInterval;

    new class Instance : ResourceProcessor.Instance
    {
        readonly float _timeInterval;

        float _consumedTime;

        public Instance(TimeConsumer node)
        {
            _timeInterval = node.TimeInterval;
        }

        public override float Progress => Mathf.Clamp01(_consumedTime / _timeInterval);

        public override void OnUpdate()
        {
            if (_consumedTime < _timeInterval)
                _consumedTime += Time.deltaTime;
        }

        public override void OnReset()
        {
            _consumedTime -= _timeInterval;
        }
    }

    protected override ResourceProcessor.Instance Instantiate() => new Instance(this);
}
