using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Consumers")]
public class IterationConsumer : ResourceProcessor
{
    [Serialize, Inspectable, UnitHeaderInspectable("Count")]
    public uint TargetCount = 1;

    new class Instance : ResourceProcessor.Instance
    {
        readonly uint _targetCount;

        uint _currentCount;

        public Instance(IterationConsumer node)
        {
            _targetCount = node.TargetCount;
        }

        public override float Progress => Mathf.Clamp01(_currentCount / (float)_targetCount);

        public override void OnUpdate()
        {
            if (_currentCount < _targetCount)
                _currentCount++;
        }

        public override void OnReset()
        {
            _currentCount = 0;
        }
    }

    protected override ResourceProcessor.Instance Instantiate() => new Instance(this);
}
