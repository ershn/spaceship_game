using System.Collections.Generic;

public static class TaskExtensions
{
    public static TaskSet ToTaskSet(this IEnumerable<ITask> tasks) => new(tasks);
}
