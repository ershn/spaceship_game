using System;
using System.Threading;
using System.Threading.Tasks;

public static class AsyncUtils
{
    public static Task ToAsync(Action<Action<bool>> start, Action cancel, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<object>();
        var ctr = ct.Register(cancel);
        start(success =>
        {
            ctr.Dispose();
            if (success)
                tcs.SetResult(null);
            else
                tcs.SetCanceled();
        });
        return tcs.Task;
    }
}
