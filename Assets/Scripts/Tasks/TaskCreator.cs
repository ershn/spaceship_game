using System.Collections.Generic;
using System.Linq;

public static class TaskCreator
{
    public static ITask WorkOn(IWork work)
    {
        return new SequenceTask(
            new ITask[] { new MoveTask(work.transform.position), new WorkTask(work) }
        );
    }

    public static ITask DeliverItem(ItemAmount item, ulong amount, IInventoryAdd inventory)
    {
        new TaskNode(new MoveTask(item.transform.position), out var startNode)
            .To(new(new ItemFromWorldToBackpackTask(item, amount)))
            .To(
                new(new MoveTask(inventory.transform.position)),
                new(new DumpBackpackTask(), out var failureNode)
            )
            .To(new(new ItemFromBackpackToInventoryTask(inventory, item.Def, amount)), failureNode)
            .To(null, failureNode);

        return new GraphTask(startNode);
    }

    public static ITask EatFood(
        IEnumerable<(ItemAmount itemAmount, ulong markedAmount)> foodItems,
        IWork foodConsumption
    )
    {
        var failureNode = new TaskNode(new DumpBackpackTask());

        var item = foodItems.First();

        var startNode = new TaskNode(new MoveTask(item.itemAmount.transform.position));
        var lastNode = startNode.To(
            new(new ItemFromWorldToBackpackTask(item.itemAmount, item.markedAmount))
        );

        foreach (var (itemAmount, markedAmount) in foodItems.Skip(1))
        {
            lastNode = lastNode
                .To(new(new MoveTask(itemAmount.transform.position)), failureNode)
                .To(new(new ItemFromWorldToBackpackTask(itemAmount, markedAmount)), failureNode);
        }

        lastNode.To(new(new WorkTask(foodConsumption)), failureNode).To(null, failureNode);

        return new GraphTask(startNode);
    }
}
