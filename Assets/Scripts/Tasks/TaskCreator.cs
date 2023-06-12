using System.Collections.Generic;
using System.Linq;

using TaskNode = Vertex<SuccessState, ITask>;

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
            .Link(new(new ItemFromWorldToBackpackTask(item, amount)))
            .Link(
                new(new MoveTask(inventory.transform.position)),
                new(new DumpBackpackTask(), out var failureNode)
            )
            .Link(
                new(new ItemFromBackpackToInventoryTask(inventory, item.Def, amount)),
                failureNode
            )
            .Link(null, failureNode);

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
        var lastNode = startNode.Link(
            new(new ItemFromWorldToBackpackTask(item.itemAmount, item.markedAmount))
        );

        foreach (var (itemAmount, markedAmount) in foodItems.Skip(1))
        {
            lastNode = lastNode
                .Link(new(new MoveTask(itemAmount.transform.position)), failureNode)
                .Link(new(new ItemFromWorldToBackpackTask(itemAmount, markedAmount)), failureNode);
        }

        lastNode.Link(new(new WorkTask(foodConsumption)), failureNode).Link(null, failureNode);

        return new GraphTask(startNode);
    }
}
