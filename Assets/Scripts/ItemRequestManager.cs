using System.Linq;
using UnityEngine;

public class ItemRequestManager : MonoBehaviour
{
    public TaskRequestEvent OnTaskCreation;

    public ItemGridIndexer ItemGrid;

    void OnEnable()
    {
        // ItemGrid.OnItemAdded.AddListener();
        // ItemGrid.OnItemRemoved.AddListener();
        ConstructionRequester.OnItemRequest += RequestItemDelivery;
    }

    void OnDisable()
    {
        ConstructionRequester.OnItemRequest -= RequestItemDelivery;
    }

    // TODO: handle canceled tasks
    // TODO: select the closest items
    void RequestItemDelivery(
        ItemDef itemDef, ulong requestedAmount, IItemAmountAdd inventory
        )
    {
        var items = ItemGrid.GetAllItems(itemDef).Select(item => item.GetComponent<IAmount>());

        foreach (var item in items)
        {
            var availableAmount = item.Get();
            if (availableAmount == 0)
                continue;

            var markedAmount = requestedAmount >= availableAmount
                ? availableAmount : requestedAmount;
            item.Reserve(markedAmount);
            requestedAmount -= markedAmount;

            DispatchItemDeliveryTask(itemDef, markedAmount, item, inventory);

            if (requestedAmount == 0)
                break;
        }
    }

    void DispatchItemDeliveryTask(
        ItemDef itemDef, ulong amount, IAmountRemove item, IItemAmountAdd inventory
        )
    {
        Debug.Log($"Request item delivery: {itemDef}, {amount}");
        var task = new CompositeTask(new ITask[]
        {
            new MoveTask(item.transform.position),
            new ItemToInventoryTask(item, itemDef, amount),
            new MoveTask(inventory.transform.position),
            new InventoryToInventoryTask(inventory, itemDef, amount)
        });
        OnTaskCreation.Invoke(task, success => {});
    }
}