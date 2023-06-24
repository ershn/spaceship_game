using System;

public static class FunctionalUtils
{
    public static Action<T1> Do<T1>(Action f, Action<T1> g) =>
        x =>
        {
            f();
            g(x);
        };

    public static Action<T1> Compose<T1>(Action f, Action<T1> g) =>
        x =>
        {
            g(x);
            f();
        };

    public static Action Compose(Action f, Action g) =>
        () =>
        {
            g();
            f();
        };

    public static Func<T2, R> Partial<T1, T2, R>(Func<T1, T2, R> f, T1 t1) => t2 => f(t1, t2);

    public static Func<T2, T3, R> Partial<T1, T2, T3, R>(Func<T1, T2, T3, R> f, T1 t1) =>
        (t2, t3) => f(t1, t2, t3);
}
