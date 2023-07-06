using System.Collections.Generic;

public static class TaskExtensions
{
    public static TaskSet ToTaskSet(this IEnumerable<Task> tasks) => new(tasks);
}
