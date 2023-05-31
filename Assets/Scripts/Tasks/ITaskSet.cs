using System.Collections.Generic;

public interface ITaskSet
{
    void Cancel();

    IEnumerable<ITask> AsEnumerable();
}
