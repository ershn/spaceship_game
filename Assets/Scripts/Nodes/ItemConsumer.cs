using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Consumers")]
public class ItemConsumer : ResourceProcessor
{
    [Serialize, Inspectable, UnitHeaderInspectable("ItemDef")]
    public ItemDef ItemDef;

    [Serialize, Inspectable, UnitHeaderInspectable("Amount")]
    public ulong Amount;

    new class Instance : ResourceProcessor.Instance
    {
        readonly ItemDef _itemDef;
        readonly ulong _amount;

        StructureResourceInventory _inventory;

        bool _completed;

        public Instance(ItemConsumer node)
        {
            _itemDef = node.ItemDef;
            _amount = node.Amount;
        }

        public override bool Completed => _completed;

        public override void OnStart()
        {
            _inventory = GameObject.GetComponent<StructureResourceInventory>();
        }

        public override void OnUpdate()
        {
            _completed = _inventory.TryRemove(_itemDef, _amount);
        }

        public override void OnReset()
        {
            _completed = false;
        }
    }

    protected override ResourceProcessor.Instance Instantiate() => new Instance(this);
}
