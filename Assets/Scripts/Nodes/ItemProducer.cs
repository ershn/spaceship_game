using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Producers")]
public class ItemProducer : ResourceProcessor
{
    [Serialize, Inspectable, UnitHeaderInspectable("Probability")]
    public float Probability;

    [Serialize, Inspectable, UnitHeaderInspectable("ItemDef")]
    public ItemDef ItemDef;

    [Serialize, Inspectable, UnitHeaderInspectable("Amount")]
    public ulong Amount;

    new class Instance : ResourceProcessor.Instance
    {
        readonly float _probability;
        readonly ItemDef _itemDef;
        readonly ulong _amount;

        ItemCreator _itemCreator;
        GridPosition _gridPosition;

        bool _completed;

        public Instance(ItemProducer node)
        {
            _probability = node.Probability;
            _itemDef = node.ItemDef;
            _amount = node.Amount;
        }

        public override bool Completed => _completed;

        public override void OnStart()
        {
            _itemCreator = GameObject.GetComponentInParent<WorldInternalIO>().ItemCreator;
            _gridPosition = GameObject.GetComponent<GridPosition>();
        }

        public override void OnUpdate()
        {
            if (Random.value <= _probability)
            {
                _itemCreator.Create(_gridPosition.CellPosition, _itemDef, _amount);
                _completed = true;
            }
        }

        public override void OnReset()
        {
            _completed = false;
        }
    }

    protected override ResourceProcessor.Instance Instantiate() => new Instance(this);
}
